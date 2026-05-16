using BaseInfrastructure.Factories;
using Main.Domain.Tests.Models;

namespace Main.Infrastructure.Repositories.Tests;

public class SessionRepository : ISessionRepository
{
    private readonly IRepositoryFactory<Session, Guid> _repositoryFactory;

    public SessionRepository(IRepositoryFactory<Session, Guid> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }
    
    public IEnumerable<Session> GetSessionsByUserId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Session>> GetSessionsByUserIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Session GetSessionById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Session> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}