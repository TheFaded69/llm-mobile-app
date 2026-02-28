using Users.Domain.Enums;

namespace Users.Contracts.V1.Responses;

public class GetUserResponse
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public UserRole Role { get; set; }
    
    public UserType UserType { get; set; }
}