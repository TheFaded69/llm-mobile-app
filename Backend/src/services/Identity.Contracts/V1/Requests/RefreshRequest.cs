namespace Identity.Contracts.V1.Requests;

public class RefreshRequest
{
    public string RefreshToken { get; set; } = default!;
}
