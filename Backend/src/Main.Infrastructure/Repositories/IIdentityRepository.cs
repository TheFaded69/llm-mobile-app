using Main.Domain.Identity.Models;

namespace Main.Infrastructure.Repositories;

public interface IIdentityRepository
{
    Task<IdentityUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IdentityUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IdentityUser> AddUserAsync(IdentityUser user, CancellationToken cancellationToken);
    Task UpdateUserAsync(IdentityUser user, CancellationToken cancellationToken);
    Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken);
    Task RevokeRefreshTokenAsync(Guid tokenId, CancellationToken cancellationToken);
    Task<PasswordResetToken> AddPasswordResetTokenAsync(PasswordResetToken resetToken, CancellationToken cancellationToken);
    Task<PasswordResetToken?> GetPasswordResetTokenByHashAsync(string tokenHash, CancellationToken cancellationToken);
    Task MarkPasswordResetTokenUsedAsync(Guid tokenId, CancellationToken cancellationToken);
    Task<ExternalLogin?> GetExternalLoginAsync(string provider, string providerUserId, CancellationToken cancellationToken);
    Task AddExternalLoginAsync(ExternalLogin externalLogin, CancellationToken cancellationToken);
}
