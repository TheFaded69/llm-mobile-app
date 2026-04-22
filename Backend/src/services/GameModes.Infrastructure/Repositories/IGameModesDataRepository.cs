using GameModes.Contracts.V1;
using GameModes.Domain.Models;

namespace GameModes.Infrastructure.Repositories;

public interface IGameModesDataRepository
{
    IReadOnlyList<TestModeMeta> GetModes();
    TestFiltersResponse GetFilters();
    IReadOnlyList<TestSetAggregate> GetSetsByMode(string mode);
    TestSetAggregate? GetSetByModeAndId(string mode, string setId);
    TestSessionAggregate CreateSession(string mode, string setId);
    TestSessionAggregate? GetSession(string mode, string sessionId);
    TestSessionAggregate? SubmitSession(string mode, string sessionId, List<SessionAnswerDto> answers, DateTimeOffset? finishedAt);
    (int CorrectCount, List<ResultItemDto> Items)? GetResultItems(string mode, string sessionId);
}
