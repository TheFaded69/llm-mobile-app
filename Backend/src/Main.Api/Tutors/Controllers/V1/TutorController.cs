using Main.Api.Extensions;
using Main.Application.Tutors;
using Main.Contract.Tutors.V1.Requests;
using Main.Contract.Tutors.V1.Responses;
using Main.Domain.Tutors.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api.Tutors.Controllers.V1;

[ApiController]
[Route("api/v1/tutors")]
public class TutorController : ControllerBase
{
    private readonly ITutorService _tutorService;

    public TutorController(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetTutorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            var response = await _tutorService.GetTutors(userId, cancellationToken);
            
            if (response == null)
                return NotFound();
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(GetTutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTutor(
        Guid id, 
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _tutorService.GetTutor(id, cancellationToken);

            if (response == null)
                return NotFound();
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("my")]
    [Authorize]
    [ProducesResponseType(typeof(GetTutorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            var response = await _tutorService.GetMyTutors(userId, cancellationToken);

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
    [ProducesResponseType(typeof(GetTutorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFavoriteTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            var response = await _tutorService.GetFavoriteTutors(userId, cancellationToken);

            if (response == null)
                return NotFound();
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Tutor), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTutor(
        [FromBody] CreateTutorRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var userId = User.GetUserId();
            
            await _tutorService.CreateTutor(userId, request, cancellationToken);
            
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTutor(
        Guid id, 
        [FromBody] UpdateTutorRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Tutor.Id != Guid.Empty && request.Tutor.Id != id)
                return BadRequest("Не совпадает [ID] в запросе и теле");
            
            await _tutorService.UpdateTutor(id, request, cancellationToken);
            
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
    public async Task<IActionResult> DeleteTutor(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _tutorService.DeleteTutor(id, cancellationToken);
            
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}