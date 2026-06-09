using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Identity.V1.Requests;

public class ResetPasswordRequest
{
    [Required]
    public string ResetToken { get; set; } = default!;
    
    [Required]
    public string NewPassword { get; set; } = default!;
}
