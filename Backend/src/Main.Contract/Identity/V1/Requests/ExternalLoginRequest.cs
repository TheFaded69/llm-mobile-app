using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Identity.V1.Requests;

public class ExternalLoginRequest
{
    [Required]
    public string? IdToken { get; set; }
    
    [Required]
    public string? IdentityToken { get; set; }
    
    [Required]
    public string? ServerAuthCode { get; set; }
    
    [Required]
    public string? AuthorizationCode { get; set; }
    
    [Required]
    public string? Username { get; set; }
}
