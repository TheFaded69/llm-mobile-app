using Main.Api.Extensions;
using Main.Application.Tests;
using Main.Contract.Tests.V1.Responses;
using Main.Domain.Tests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api.Tests.Controllers.V1;

[ApiController]
[Route("tests")]
public class TestController : ControllerBase
{
    private readonly ISetService _setService;

    public TestController(ISetService setService)
    {
        _setService = setService;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Set), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSet(
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        
        var id = await _setService.CreateSetAsync(userId, cancellationToken);

        return CreatedAtAction(
            nameof(GetSet),
            new { id },
            new { id }
        );
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(GetSetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSet(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _setService.GetSetByIdAsync(id, cancellationToken);
        
        if (response == null)
            return NotFound();
            
        return Ok(response);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetSetsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSets(
        CancellationToken cancellationToken
    )
    {
        var userId = User.GetUserId();

        var response = await _setService.GetSetsAsync(userId, cancellationToken);
        
        if (response == null)
            return NotFound();
        
        return Ok(response);
    }
}