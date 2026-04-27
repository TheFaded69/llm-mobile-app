namespace Main.Contract.Identity.V1.Requests;

public class RegisterRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Username { get; set; } = default!;
}
