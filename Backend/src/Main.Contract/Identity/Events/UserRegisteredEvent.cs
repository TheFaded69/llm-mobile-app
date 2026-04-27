namespace Main.Contract.Identity.Events;

public class UserRegisteredEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
}
