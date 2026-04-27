namespace Main.Application.Identity.Services;

public interface IUserRegisteredPublisher
{
    Task PublishAsync(Guid userId, string email, string username, CancellationToken cancellationToken);
}
