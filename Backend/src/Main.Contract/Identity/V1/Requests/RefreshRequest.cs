namespace Main.Contract.Identity.V1.Requests;

public class RefreshRequest
{
    public string RefreshToken { get; set; } = default!;
}
