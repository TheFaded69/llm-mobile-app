using BaseInfrastructure.Factories;
using Main.Domain.Tests.Enums;
using Main.Domain.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Repositories.Tests;

public class SetRepository : ISetRepository
{
    private readonly IRepositoryFactory<Set, Guid> _repositoryFactory;

    public SetRepository(IRepositoryFactory<Set, Guid> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }
    
    public async Task<IEnumerable<Set>> GetSetsAsync(
        IEnumerable<TestDifficult> difficults,
        IEnumerable<SetStatus> setStatuses,
        CancellationToken cancellationToken,
        bool includePrivate = false)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();

        var query = repository.Query;

        if (difficults != null && difficults.Any())
        {
            query = query.Where(s => difficults.Contains(s.TestDifficult));
        }
        
        if (setStatuses != null && setStatuses.Any())
        {
            query = query.Where(s => setStatuses.Contains(s.SetStatus));
        }

        if (!includePrivate)
        {
            query = query.Where(s => s.IsPublic);
        }

        return await query.ToListAsync(cancellationToken);;
    }

    public async Task<IEnumerable<Set>> GetSetsByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.Query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Set>> GetSetsByUserIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository
            .Query
            .Where(x => x.UserId == id)
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<IEnumerable<Set>> GetFavoriteSetsByUserIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task AddSetAsync(
        Set set,
        CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        repository.Insert(set);
        
        await repository.CommitAsync();
    }
    
    public async Task DeleteSetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        var existSet = await repository.GetAsync(id, cancellationToken) ?? throw new Exception($"Такого набора [{id}] не существует");
        
        repository.Delete(existSet);
        
        await repository.CommitAsync();
    }

    public async Task UpdateSetAsync(
        Set set,
        CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        repository.Update(set);
        
        await repository.CommitAsync();
    }

    public async Task<Set> GetSetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.GetAsync(id, cancellationToken);
    }
}