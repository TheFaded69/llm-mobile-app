namespace Identity.Contracts.V1.Responses;

public class ForgotPasswordResponse
{
    public string ResetToken { get; set; } = default!;
    public DateTimeOffset ExpiresAt { get; set; }
}
