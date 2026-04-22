namespace Identity.Application.Services;

public interface IUserRegisteredPublisher
{
    Task PublishAsync(Guid userId, string email, string username, CancellationToken cancellationToken);
}
