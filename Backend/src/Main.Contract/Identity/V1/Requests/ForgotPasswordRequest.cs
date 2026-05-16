namespace Main.Contract.Identity.V1.Requests;

public class ForgotPasswordRequest
{
    public string Email { get; set; } = default!;
}
