using BaseDomain.Models;

namespace Main.Domain.Identity.Models;

public class IdentityUser : GuidEntity
{
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string UserName { get; set; } = default!;
}
