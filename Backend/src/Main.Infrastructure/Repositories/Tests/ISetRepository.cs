using Main.Domain.Tests.Enums;
using Main.Domain.Tests.Models;

namespace Main.Infrastructure.Repositories.Tests;

public interface ISetRepository
{
   
    Task<IEnumerable<Set>> GetSetsAsync(
        IEnumerable<TestDifficult> difficults,
        IEnumerable<SetStatus> setStatuses,
        CancellationToken cancellationToken,
        bool includePrivate = false);
    
    Task<IEnumerable<Set>> GetSetsByIdsAsync(
        IEnumerable<Guid> ids, 
        CancellationToken cancellationToken);
    
    Task<IEnumerable<Set>> GetSetsByUserIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task AddSetAsync(
        Set set,
        CancellationToken cancellationToken);
    
    Task DeleteSetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task UpdateSetAsync(
        Set set,
        CancellationToken cancellationToken);
    
    Task<Set?> GetSetByIdAsync(Guid id, CancellationToken cancellationToken);
}