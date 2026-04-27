namespace Main.Contract.Identity.V1.Requests;

public class ExternalLoginRequest
{
    public string Code { get; set; } = default!;
    public string RedirectUri { get; set; } = default!;
    public string CodeVerifier { get; set; } = default!;
    public string? Username { get; set; }
}
