using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Main.Application.Identity.Models;
using Main.Contract.Identity.V1.Responses;
using Main.Domain.Identity.Models;
using Main.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Main.Application.Identity.Services;

public class AuthService
{
    private readonly IIdentityRepository _repository;
    private readonly JwtOptions _jwtOptions;
    private readonly IUserRegisteredPublisher _publisher;
    private readonly PasswordHasher<IdentityUser> _passwordHasher = new();

    public AuthService(
        IIdentityRepository repository,
        IOptions<JwtOptions> jwtOptions,
        IUserRegisteredPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthTokensResponse> RegisterAsync(string email, string password, string username, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetUserByEmailAsync(email, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException("Пользователь с таким email уже существует.");

        var user = new IdentityUser
        {
            Email = email,
            UserName = username
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        await _repository.AddUserAsync(user, cancellationToken);
        await _publisher.PublishAsync(user.Id, user.Email, user.UserName, cancellationToken);

        return await CreateTokensAsync(user, cancellationToken);
    }

    public async Task<AuthTokensResponse> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByEmailAsync(email, cancellationToken)
                   ?? throw new InvalidOperationException("Неверный email или пароль.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            throw new InvalidOperationException("Неверный email или пароль.");

        return await CreateTokensAsync(user, cancellationToken);
    }

    public async Task<AuthTokensResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenHash = ComputeHash(refreshToken);
        var existing = await _repository.GetRefreshTokenByHashAsync(refreshTokenHash, cancellationToken)
                       ?? throw new InvalidOperationException("Refresh token не найден.");

        if (existing.RevokedAt != null || existing.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Refresh token недействителен.");

        var user = await _repository.GetUserByIdAsync(existing.IdentityUserId, cancellationToken)
                   ?? throw new InvalidOperationException("Пользователь не найден.");

        await _repository.RevokeRefreshTokenAsync(existing.Id, cancellationToken);

        return await CreateTokensAsync(user, cancellationToken);
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByEmailAsync(email, cancellationToken);
        if (user == null)
        {
            return new ForgotPasswordResponse
            {
                ResetToken = string.Empty,
                ExpiresAt = DateTimeOffset.UtcNow
            };
        }

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(30);

        await _repository.AddPasswordResetTokenAsync(new PasswordResetToken
        {
            IdentityUserId = user.Id,
            TokenHash = ComputeHash(token),
            ExpiresAt = expiresAt
        }, cancellationToken);

        return new ForgotPasswordResponse
        {
            ResetToken = token,
            ExpiresAt = expiresAt
        };
    }

    public async Task ResetPasswordAsync(string resetToken, string newPassword, CancellationToken cancellationToken)
    {
        var hash = ComputeHash(resetToken);
        var token = await _repository.GetPasswordResetTokenByHashAsync(hash, cancellationToken)
                    ?? throw new InvalidOperationException("Reset token не найден.");

        if (token.UsedAt != null || token.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Reset token недействителен.");

        var user = await _repository.GetUserByIdAsync(token.IdentityUserId, cancellationToken)
                   ?? throw new InvalidOperationException("Пользователь не найден.");

        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
        await _repository.UpdateUserAsync(user, cancellationToken);
        await _repository.MarkPasswordResetTokenUsedAsync(token.Id, cancellationToken);
    }

    public async Task<AuthTokensResponse> ExternalLoginAsync(string provider, string code, string? username, CancellationToken cancellationToken)
    {
        var providerUserId = code;
        var external = await _repository.GetExternalLoginAsync(provider, providerUserId, cancellationToken);

        IdentityUser user;
        if (external != null)
        {
            user = await _repository.GetUserByIdAsync(external.IdentityUserId, cancellationToken)
                   ?? throw new InvalidOperationException("Пользователь внешней аутентификации не найден.");
        }
        else
        {
            var email = $"{providerUserId}@{provider}.external".ToLowerInvariant();
            user = await _repository.GetUserByEmailAsync(email, cancellationToken) ?? new IdentityUser
            {
                Email = email,
                UserName = username ?? $"{provider}_{providerUserId[..Math.Min(8, providerUserId.Length)]}"
            };

            if (user.Id == Guid.Empty)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, Convert.ToBase64String(RandomNumberGenerator.GetBytes(24)));
                await _repository.AddUserAsync(user, cancellationToken);
                await _publisher.PublishAsync(user.Id, user.Email, user.UserName, cancellationToken);
            }

            await _repository.AddExternalLoginAsync(new ExternalLogin
            {
                IdentityUserId = user.Id,
                Provider = provider,
                ProviderUserId = providerUserId
            }, cancellationToken);
        }

        return await CreateTokensAsync(user, cancellationToken);
    }

    private async Task<AuthTokensResponse> CreateTokensAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var accessExpires = now.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var refreshExpires = now.AddDays(_jwtOptions.RefreshTokenDays);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("username", user.UserName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: accessExpires.UtcDateTime,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        await _repository.AddRefreshTokenAsync(new RefreshToken
        {
            IdentityUserId = user.Id,
            TokenHash = ComputeHash(refreshToken),
            ExpiresAt = refreshExpires
        }, cancellationToken);

        return new AuthTokensResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessExpires,
            RefreshTokenExpiresAt = refreshExpires
        };
    }

    private static string ComputeHash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}
