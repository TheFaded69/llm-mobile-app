using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Identity.V1.Requests;

public class LoginRequest
{
    [Required]
    public string Email { get; set; } = default!;
    
    [Required]
    public string Password { get; set; } = default!;
}
