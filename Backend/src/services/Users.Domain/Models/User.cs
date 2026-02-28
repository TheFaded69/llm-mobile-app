using BaseDomain.Models;
using Users.Domain.Enums;

namespace Users.Domain.Models;

public class User : GuidEntity
{
    public UserRole Role { get; set; }
    
    public UserType UserType { get; set; }
    
    public string Email { get; set; }
    
    public string UserName { get; set; }
}