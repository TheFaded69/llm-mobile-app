using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Identity.V1.Requests;

public class ForgotPasswordRequest
{
    [Required]
    public string Email { get; set; } = default!;
}
