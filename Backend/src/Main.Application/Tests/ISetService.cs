using Main.Contract.Tests.V1.Responses;
using Main.Domain.Tests.Models;

namespace Main.Application.Tests;

public interface ISetService
{
    Task<Guid> CreateSetAsync(Guid userId, CancellationToken cancellationToken);
    
    Task AddSetAsync(CancellationToken cancellationToken);
    
    Task<GetSetResponse> GetSetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<GetSetsResponse?> GetSetsAsync(Guid userId, CancellationToken cancellationToken);
}