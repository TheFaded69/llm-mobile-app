using BaseDomain.Models;

namespace Identity.Domain.Models;

public class RefreshToken : GuidEntity
{
    public Guid IdentityUserId { get; set; }
    
    public string TokenHash { get; set; } = default!;
    
    public DateTimeOffset ExpiresAt { get; set; }
    
    public DateTimeOffset? RevokedAt { get; set; }
    
    public string? UserAgent { get; set; }
    
    public string? DeviceId { get; set; }
}
