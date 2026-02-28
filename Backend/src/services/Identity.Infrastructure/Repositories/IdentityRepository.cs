using Identity.Domain.Models;
using Identity.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class IdentityRepository : IIdentityRepository
{
    private readonly IdentityDataContext _context;

    public IdentityRepository(IdentityDataContext context)
    {
        _context = context;
    }

    public Task<IdentityUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        => _context.Set<IdentityUser>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<IdentityUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        => _context.Set<IdentityUser>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IdentityUser> AddUserAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        _context.Set<IdentityUser>().Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task UpdateUserAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        _context.Set<IdentityUser>().Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        _context.Set<RefreshToken>().Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);
        return refreshToken;
    }

    public Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken)
        => _context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

    public async Task RevokeRefreshTokenAsync(Guid tokenId, CancellationToken cancellationToken)
    {
        var token = await _context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == tokenId, cancellationToken)
                    ?? throw new InvalidOperationException("Refresh token not found.");

        token.RevokedAt = DateTimeOffset.UtcNow;
        _context.Set<RefreshToken>().Update(token);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PasswordResetToken> AddPasswordResetTokenAsync(PasswordResetToken resetToken, CancellationToken cancellationToken)
    {
        _context.Set<PasswordResetToken>().Add(resetToken);
        await _context.SaveChangesAsync(cancellationToken);
        return resetToken;
    }

    public Task<PasswordResetToken?> GetPasswordResetTokenByHashAsync(string tokenHash, CancellationToken cancellationToken)
        => _context.Set<PasswordResetToken>().FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

    public async Task MarkPasswordResetTokenUsedAsync(Guid tokenId, CancellationToken cancellationToken)
    {
        var token = await _context.Set<PasswordResetToken>().FirstOrDefaultAsync(x => x.Id == tokenId, cancellationToken)
                    ?? throw new InvalidOperationException("Reset token not found.");

        token.UsedAt = DateTimeOffset.UtcNow;
        _context.Set<PasswordResetToken>().Update(token);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<ExternalLogin?> GetExternalLoginAsync(string provider, string providerUserId, CancellationToken cancellationToken)
        => _context.Set<ExternalLogin>().FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderUserId == providerUserId, cancellationToken);

    public async Task AddExternalLoginAsync(ExternalLogin externalLogin, CancellationToken cancellationToken)
    {
        _context.Set<ExternalLogin>().Add(externalLogin);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
