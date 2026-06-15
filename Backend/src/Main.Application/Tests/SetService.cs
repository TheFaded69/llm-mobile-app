using Main.Contract.Tests.V1.DTO;
using Main.Contract.Tests.V1.Requests;
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
    private readonly ISessionRepository _sessionRepository;
    private readonly IFavoriteSetRepository _favoriteSetRepository;

    public SetService(
        ISetRepository setRepository,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IFavoriteSetRepository favoriteSetRepository
        )
    {
        _setRepository = setRepository;
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _favoriteSetRepository = favoriteSetRepository;
    }
    
    public async Task<Guid> CreateSetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var set = new Set()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = string.Empty,
            Description = string.Empty,
            SetStatus = SetStatus.Draft,
            TestDifficult = TestDifficult.None,
        };
        
        await _setRepository.AddSetAsync(set, cancellationToken);
        
        return set.Id;
    }
    

    public async Task<GetSetResponse> GetSetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var set = await _setRepository.GetSetByIdAsync(id, cancellationToken);
        
        if (set == null) return null;

        var setDto = new SetDTO
        {
            Id = set.Id,
            Title = set.Title,
            Description = set.Description,
            TestDifficult = set.TestDifficult,
            Duration = set.Duration,
            SetItems = set.SetItems
                .Select(setItem => new SetItemDTO
                {
                    Id = setItem.Id,
                    SetId = set.Id,
                    Term = setItem.Term,
                    Description = setItem.Description,
                })
                .ToList(),
            
        };

        return new GetSetResponse()
        {
            Set = setDto
        };
    }

    public async Task<GetSetsResponse?> GetSetsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var sets = await _setRepository.GetSetsAsync(null, null, cancellationToken);
        var sessions = await _sessionRepository.GetSessionsByUserIdAsync(userId, cancellationToken);

        var response = new GetSetsResponse
        {
            Sets = sets.Select(set => new SetDTO
                {
                    Id = set.Id,
                    Title = set.Title,
                    Description = set.Description,
                    TestDifficult = set.TestDifficult,
                    Duration = set.Duration,
                    SetItems = set.SetItems
                        .Select(setItem => new SetItemDTO
                        {
                            Id = setItem.Id,
                            SetId = set.Id,
                            Term = setItem.Term,
                            Description = setItem.Description,
                        })
                        .ToList(),
                    Sessions = sessions.Select(session => new SessionDTO
                    {
                        Id = session.Id,
                        SetId = session.SetId,
                        TestMode = session.TestMode,
                        SessionStatus = session.SessionStatus,
                        Progress = 0,
                        SessionItems = session.SessionItems.Select(sessionItem => new SessionItemDTO
                        {
                            Id = sessionItem.Id,
                            SessionId =  session.Id,
                            Question = new QuestionDTO
                            {
                                Definition = sessionItem.Question.Definition,
                                ExplainTerm = sessionItem.Question.ExplainTerm,
                                ExplainText = sessionItem.Question.ExplainText
                            },
                            Answer = new  AnswerDTO()
                            {
                                
                            },
                            IsCorrect = null
                        }).ToList()
                    }).ToList(),
                })
                .ToList(),
        };

        return response;
    }

    public async Task UpdateSetAsync(UpdateSetRequest updateSetRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteSetAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<GetSetsResponse?> GetMySetsAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<GetSetsResponse?> GetFavoriteSetsAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}