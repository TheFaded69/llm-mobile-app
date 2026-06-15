using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Identity.V1.Requests;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; } = default!;
}
