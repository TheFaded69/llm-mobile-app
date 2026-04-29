using Main.Contract.Tests.V1.Responses;
using Main.Domain.Tests.Enums;
using Main.Domain.Tests.Models;
using Main.Infrastructure.Repositories;
using Main.Infrastructure.Repositories.Tests;

namespace Main.Application.Tests;

public class SetService : ISetService
{
    private readonly ISetRepository _setRepository;
    private readonly IUserRepository _userRepository;

    public SetService(ISetRepository? setRepository,
        IUserRepository userRepository)
    {
        _setRepository = setRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Guid> CreateSetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user =  await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        var set = new Set()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            User = user,
            Title = "Новый набор",
            Description = string.Empty,
            SetStatus = SetStatus.Draft,
            TestDifficult = TestDifficult.None,
        };
        
        await _setRepository.AddSetAsync(set, cancellationToken);
        
        return set.Id;
    }

    public async Task AddSetAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<GetSetResponse> GetSetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var set = await _setRepository.GetSetByIdAsync(id, cancellationToken);
        
        if (set == null) return null;
        
        return new GetSetResponse()
        {
            Id = set.Id,
        };
    }
}