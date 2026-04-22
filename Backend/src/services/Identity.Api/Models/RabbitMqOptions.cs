namespace Identity.Api.Models;

public class RabbitMqOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "identity.events";
    public string UserRegisteredRoutingKey { get; set; } = "user.registered";
}
