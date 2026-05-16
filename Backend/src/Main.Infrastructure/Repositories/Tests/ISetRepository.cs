using Main.Domain.Tests.Enums;
using Main.Domain.Tests.Models;

namespace Main.Infrastructure.Repositories.Tests;

public interface ISetRepository
{
    IEnumerable<Set> GetSets(
        IEnumerable<TestDifficult> difficults,
        IEnumerable<SessionStatus> sessionStatuses,
        IEnumerable<SetStatus> setStatuses,
        bool includePrivate = false);
    
    Task<IEnumerable<Set>> GetSetsAsync(
        IEnumerable<TestDifficult> difficults,
        IEnumerable<SessionStatus> sessionStatuses,
        IEnumerable<SetStatus> setStatuses,
        CancellationToken cancellationToken,
        bool includePrivate = false);
    
    IEnumerable<Set> GetSetsByUserId(Guid id);
    
    Task<IEnumerable<Set>> GetSetsByUserIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    IEnumerable<Set> GetFavoriteSetsByUserId(Guid sessionId);
    
    Task<IEnumerable<Set>> GetFavoriteSetsByUserIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken);
    
    void AddSet(Set set);
    
    Task AddSetAsync(
        Set set,
        CancellationToken cancellationToken);
    
    void DeleteSet(Guid id);
    
    Task DeleteSetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    void UpdateSet(Set set);
    
    Task UpdateSetAsync(
        Set set,
        CancellationToken cancellationToken);
    
    Set GetSetById(Guid id);
    
    Task<Set> GetSetByIdAsync(Guid id, CancellationToken cancellationToken);
}