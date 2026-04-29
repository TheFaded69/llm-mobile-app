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

    public IEnumerable<Set> GetSets(
        IEnumerable<TestDifficult> difficults,
        IEnumerable<SessionStatus> sessionStatuses,
        IEnumerable<SetStatus> setStatuses, 
        bool includePrivate = false)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Set>> GetSetsAsync(
        IEnumerable<TestDifficult> difficults,
        IEnumerable<SessionStatus> sessionStatuses, 
        IEnumerable<SetStatus> setStatuses,
        CancellationToken cancellationToken,
        bool includePrivate = false)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Set> GetSetsByUserId(Guid id)
    {
        using var repository = _repositoryFactory.CreateRepository();
        
        return repository
            .Query
            .Where(x => x.UserId == id)
            .ToList();
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

    public IEnumerable<Set> GetFavoriteSetsByUserId(Guid sessionId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Set>> GetFavoriteSetsByUserIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void AddSet(Set set)
    {
        using var repository = _repositoryFactory.CreateRepository();
        
        repository.Insert(set);
        
        repository.Commit();
    }

    public async Task AddSetAsync(
        Set set,
        CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        repository.Insert(set);
        
        await repository.CommitAsync();
    }

    public void DeleteSet(Guid id)
    {
        using var repository = _repositoryFactory.CreateRepository();
        
        var existSet = repository.Get(id) ?? throw new Exception($"Такого набора [{id}] не существует");
        
        repository.Delete(existSet);
        
        repository.Commit();
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

    public void UpdateSet(Set set)
    {
        using var repository = _repositoryFactory.CreateRepository();
        
        repository.Update(set);
        
        repository.Commit();
    }

    public async Task UpdateSetAsync(
        Set set,
        CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        repository.Update(set);
        
        await repository.CommitAsync();
    }

    public Set GetSetById(Guid id)
    {
        using var repository = _repositoryFactory.CreateRepository();
        
        return repository.Get(id);
    }

    public async Task<Set> GetSetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.GetAsync(id, cancellationToken);
    }
}