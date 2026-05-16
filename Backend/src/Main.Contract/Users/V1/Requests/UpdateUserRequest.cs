using Main.Domain.Users.Enums;

namespace Main.Contract.Users.V1.Requests;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public UserRole Role { get; set; }
    
    public UserType UserType { get; set; }
}