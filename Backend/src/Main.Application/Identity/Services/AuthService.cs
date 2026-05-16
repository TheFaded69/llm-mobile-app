using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Main.Application.Identity.Models;
using Main.Contract.Identity.V1.Responses;
using Main.Domain.Identity.Models;
using Main.Domain.Users.Enums;
using Main.Domain.Users.Models;
using Main.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Main.Application.Identity.Services;

public class AuthService
{
    private readonly IIdentityRepository _identityUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly JwtOptions _jwtOptions;
    private readonly PasswordHasher<IdentityUser> _passwordHasher = new();

    public AuthService(
        IIdentityRepository identityUserRepository,
        IUserRepository userRepository,
        IOptions<JwtOptions> jwtOptions)
    {
        _identityUserRepository = identityUserRepository;
        _userRepository = userRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthTokensResponse> RegisterAsync(string email, string password, string username, CancellationToken cancellationToken)
    {
        var existing = await _identityUserRepository.GetUserByEmailAsync(email, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException("Пользователь с таким email уже существует.");

        var identityUser = new IdentityUser
        {
            Email = email,
            UserName = username
        };
        identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, password);

        await _identityUserRepository.AddUserAsync(identityUser, cancellationToken);

        var user = new User
        {
            Id = identityUser.Id,
            Role = UserRole.User,
            UserType = UserType.Student,
            Email = identityUser.Email,
            UserName = identityUser.UserName
        };
        
        await _userRepository.AddUserAsync(user, cancellationToken);
        
        return await CreateTokensAsync(identityUser, cancellationToken);
    }

    public async Task<AuthTokensResponse> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _identityUserRepository.GetUserByEmailAsync(email, cancellationToken)
                   ?? throw new InvalidOperationException("Неверный email или пароль.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            throw new InvalidOperationException("Неверный email или пароль.");

        return await CreateTokensAsync(user, cancellationToken);
    }

    public async Task<AuthTokensResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenHash = ComputeHash(refreshToken);
        var existing = await _identityUserRepository.GetRefreshTokenByHashAsync(refreshTokenHash, cancellationToken)
                       ?? throw new InvalidOperationException("Refresh token не найден.");

        if (existing.RevokedAt != null || existing.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Refresh token недействителен.");

        var user = await _identityUserRepository.GetUserByIdAsync(existing.IdentityUserId, cancellationToken)
                   ?? throw new InvalidOperationException("Пользователь не найден.");

        await _identityUserRepository.RevokeRefreshTokenAsync(existing.Id, cancellationToken);

        return await CreateTokensAsync(user, cancellationToken);
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _identityUserRepository.GetUserByEmailAsync(email, cancellationToken);
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

        await _identityUserRepository.AddPasswordResetTokenAsync(new PasswordResetToken
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
        var token = await _identityUserRepository.GetPasswordResetTokenByHashAsync(hash, cancellationToken)
                    ?? throw new InvalidOperationException("Reset token не найден.");

        if (token.UsedAt != null || token.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Reset token недействителен.");

        var user = await _identityUserRepository.GetUserByIdAsync(token.IdentityUserId, cancellationToken)
                   ?? throw new InvalidOperationException("Пользователь не найден.");

        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
        await _identityUserRepository.UpdateUserAsync(user, cancellationToken);
        await _identityUserRepository.MarkPasswordResetTokenUsedAsync(token.Id, cancellationToken);
    }

    public async Task<AuthTokensResponse> ExternalLoginAsync(string provider, string code, string? username, CancellationToken cancellationToken)
    {
        var providerUserId = code;
        var external = await _identityUserRepository.GetExternalLoginAsync(provider, providerUserId, cancellationToken);

        IdentityUser identityUser;
        if (external != null)
        {
            identityUser = await _identityUserRepository.GetUserByIdAsync(external.IdentityUserId, cancellationToken)
                   ?? throw new InvalidOperationException("Пользователь внешней аутентификации не найден.");
        }
        else
        {
            var email = $"{providerUserId}@{provider}.external".ToLowerInvariant();
            identityUser = await _identityUserRepository.GetUserByEmailAsync(email, cancellationToken) ?? new IdentityUser
            {
                Email = email,
                UserName = username ?? $"{provider}_{providerUserId[..Math.Min(8, providerUserId.Length)]}"
            };

            if (identityUser.Id == Guid.Empty)
            {
                identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, Convert.ToBase64String(RandomNumberGenerator.GetBytes(24)));
                await _identityUserRepository.AddUserAsync(identityUser, cancellationToken);
            }

            await _identityUserRepository.AddExternalLoginAsync(new ExternalLogin
            {
                IdentityUserId = identityUser.Id,
                Provider = provider,
                ProviderUserId = providerUserId
            }, cancellationToken);
        }

        return await CreateTokensAsync(identityUser, cancellationToken);
    }

    private async Task<AuthTokensResponse> CreateTokensAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var accessExpires = now.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var refreshExpires = now.AddDays(_jwtOptions.RefreshTokenDays);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

        await _identityUserRepository.AddRefreshTokenAsync(new RefreshToken
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
