using Main.Domain.Tests.Models;

namespace Main.Infrastructure.Repositories.Tests;

public interface ISessionRepository
{
    IEnumerable<Session> GetSessionsByUserId(Guid id);
    
    Task<IEnumerable<Session>> GetSessionsByUserIdAsync(Guid id, CancellationToken cancellationToken);
    
    Session GetSessionById(Guid id);
    
    Task<Session> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken);
}