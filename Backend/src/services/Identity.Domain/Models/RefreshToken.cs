using BaseDomain.Models;

namespace Identity.Domain.Models;

public class RefreshToken : GuidEntity
{
    public string TokenHash { get; set; } = default!;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceId { get; set; }

    public string UserId { get; set; } = default!;
    //public IdentityUser User { get; set; } = default!;
}