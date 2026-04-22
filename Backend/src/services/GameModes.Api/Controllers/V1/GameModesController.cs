using Microsoft.AspNetCore.Mvc;
using GameModes.Application.Services;
using GameModes.Contracts.V1;

namespace GameModes.Api.Controllers.V1;

[ApiController]
[Route("v1/tests")]
public class GameModesController : ControllerBase
{
    private readonly GameModesService _gameModesService;

    public GameModesController(GameModesService gameModesService)
    {
        _gameModesService = gameModesService;
    }

    [HttpGet("modes")]
    public IActionResult GetModes()
    {
        return Ok(new { Items = _gameModesService.GetModes() });
    }

    [HttpGet("filters")]
    public IActionResult GetFilters()
    {
        return Ok(_gameModesService.GetFilters());
    }

    [HttpGet("{mode}/sets")]
    public IActionResult ListSetsByMode(
        string mode,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? query = null,
        [FromQuery] string? difficulty = null,
        [FromQuery] string? category = null,
        [FromQuery] string? sectionDate = null,
        [FromQuery] string? sort = "relevance")
    {
        if (!TestModes.All.Contains(mode))
            return NotFound(new ErrorResponse { Code = "mode_not_found", Message = "Mode not found" });

        if (page < 1 || pageSize is < 1 or > 100)
            return BadRequest(new ErrorResponse { Code = "validation_error", Message = "Invalid pagination parameters" });

        return Ok(_gameModesService.GetSetsByMode(mode, page, pageSize, query, difficulty, category, sectionDate, sort));
    }

    [HttpGet("{mode}/sets/{setId}")]
    public IActionResult GetSetDetails(string mode, string setId)
    {
        if (!TestModes.All.Contains(mode))
            return NotFound(new ErrorResponse { Code = "mode_not_found", Message = "Mode not found" });

        var response = _gameModesService.GetSetDetails(mode, setId);
        if (response == null)
            return NotFound(new ErrorResponse { Code = "set_not_found", Message = "Set not found" });

        return Ok(response);
    }

    [HttpPost("{mode}/sessions")]
    public IActionResult CreateSession(string mode, [FromBody] CreateSessionRequest request)
    {
        if (!TestModes.All.Contains(mode))
            return NotFound(new ErrorResponse { Code = "mode_not_found", Message = "Mode not found" });

        if (string.IsNullOrWhiteSpace(request.SetId))
            return BadRequest(new ErrorResponse { Code = "validation_error", Message = "setId is required" });

        var response = _gameModesService.CreateSession(mode, request);
        if (response == null)
            return NotFound(new ErrorResponse { Code = "set_not_found", Message = "Set not found" });

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet("{mode}/sessions/{sessionId}")]
    public IActionResult GetSessionState(string mode, string sessionId)
    {
        var response = _gameModesService.GetSessionState(mode, sessionId);
        if (response == null)
            return NotFound(new ErrorResponse { Code = "session_not_found", Message = "Session not found" });

        return Ok(response);
    }

    [HttpPost("{mode}/sessions/{sessionId}/submit")]
    public IActionResult SubmitSession(string mode, string sessionId, [FromBody] SubmitRequest request)
    {
        if (request.Answers.Count == 0)
            return BadRequest(new ErrorResponse { Code = "validation_error", Message = "answers should contain at least 1 item" });

        var existing = _gameModesService.GetSessionState(mode, sessionId);
        if (existing?.Status == SessionStatuses.Submitted)
            return Conflict(new ErrorResponse { Code = "session_closed", Message = "Сессия уже закрыта" });

        var response = _gameModesService.Submit(mode, sessionId, request);
        if (response == null)
            return NotFound(new ErrorResponse { Code = "session_not_found", Message = "Session not found" });

        return Ok(response);
    }

    [HttpGet("{mode}/sessions/{sessionId}/result")]
    public IActionResult GetResult(string mode, string sessionId)
    {
        var response = _gameModesService.GetResult(mode, sessionId);
        if (response == null)
            return NotFound(new ErrorResponse { Code = "result_not_found", Message = "Result not found" });

        return Ok(response);
    }

    [HttpGet("{mode}/sessions/{sessionId}/review")]
    public IActionResult GetReview(string mode, string sessionId)
    {
        var response = _gameModesService.GetReview(mode, sessionId);
        if (response == null)
            return NotFound(new ErrorResponse { Code = "review_not_found", Message = "Review not found" });

        return Ok(response);
    }
}
