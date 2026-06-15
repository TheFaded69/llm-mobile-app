using Main.Api.Extensions;
using Main.Application.Tests;
using Main.Contract.Tests.V1.Requests;
using Main.Contract.Tests.V1.Responses;
using Main.Domain.Tests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api.Tests.Controllers.V1;

[ApiController]
[Route("api/v1/tests")]
public class TestController : ControllerBase
{
    private readonly ISetService _setService;

    public TestController(ISetService setService)
    {
        _setService = setService;
    }

    [HttpPost]
    [Authorize]
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
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSet(
        Guid id, 
        [FromBody] UpdateSetRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Set.Id != Guid.Empty && request.Set.Id != id)
                return BadRequest("Не совпадает [ID] в запросе и теле");
            
            await _setService.UpdateSetAsync(request, cancellationToken);
            
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSet(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _setService.DeleteSetAsync(id, cancellationToken);
            
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("my")]
    [Authorize]
    [ProducesResponseType(typeof(GetSetsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            var response = await _setService.GetMySetsAsync(userId, cancellationToken);

            if (response == null)
                return NotFound();
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("favorite")]
    [Authorize]
    [ProducesResponseType(typeof(GetSetsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFavoriteTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            var response = await _setService.GetFavoriteSetsAsync(userId, cancellationToken);

            if (response == null)
                return NotFound();
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}