namespace Main.Contract.Identity.V1.Requests;

public class ExternalLoginRequest
{
    public string? IdToken { get; set; }
    public string? IdentityToken { get; set; }
    public string? ServerAuthCode { get; set; }
    public string? AuthorizationCode { get; set; }
    public string? Username { get; set; }
}
