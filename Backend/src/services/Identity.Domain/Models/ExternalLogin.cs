using BaseDomain.Models;

namespace Identity.Domain.Models;

public class ExternalLogin : GuidEntity
{
    public Guid IdentityUserId { get; set; }
    public string Provider { get; set; } = default!;
    public string ProviderUserId { get; set; } = default!;
}
