using BaseDomain.Models;

namespace Identity.Domain.Models;

public class PasswordResetToken : GuidEntity
{
    public Guid IdentityUserId { get; set; }
    public string TokenHash { get; set; } = default!;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? UsedAt { get; set; }
}
