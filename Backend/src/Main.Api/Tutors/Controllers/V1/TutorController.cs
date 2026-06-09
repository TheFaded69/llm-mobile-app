using Main.Api.Extensions;
using Main.Application.Tutors;
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
    //[ProducesResponseType(typeof(GetTutorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            /*var response = await _dialogService.GetDialogs(userId);
            
            if (response == null)
                return NotFound();*/
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    //[ProducesResponseType(typeof(GetTutorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTutor(
        Guid id, 
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            /*var response = await _dialogService.GetDialogs(userId);

            if (response == null)
                return NotFound();*/
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("my")]
    [Authorize]
    //[ProducesResponseType(typeof(GetTutorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            /*var response = await _dialogService.GetDialogs(userId);

            if (response == null)
                return NotFound();*/
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("favorite")]
    [Authorize]
    //[ProducesResponseType(typeof(GetTutorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFavoriteTutors(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            /*var response = await _dialogService.GetDialogs(userId);

            if (response == null)
                return NotFound();*/
            
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}