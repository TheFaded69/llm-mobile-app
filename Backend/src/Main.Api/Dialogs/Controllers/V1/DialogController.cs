using Main.Api.Extensions;
using Main.Application.Dialogs;
using Main.Contract.Dialogs.V1.Requests;
using Main.Contract.Dialogs.V1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api.Dialogs.Controllers.V1;

[ApiController]
[Route("api/v1/dialogs")]
public class DialogController : ControllerBase
{
    private readonly IDialogService _dialogService;

    public DialogController(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetDialogsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDialogs(
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.GetUserId();
            
            var response = await _dialogService.GetDialogs(userId, cancellationToken);
            
            if (response == null)
                return NotFound();
            
            return Ok(response);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("{dialogId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(GetDialogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDialog(
        Guid dialogId,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _dialogService.GetDialog(dialogId, cancellationToken);
            
            if (response == null)
                return NotFound();
            
            return Ok(response);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDialogAsync(
        [FromBody] AddDialogRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _dialogService.CreateDialog(request, cancellationToken);
            
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPut("{dialogId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDialogAsync(
        [FromBody] UpdateDialogRequest request, 
        Guid dialogId,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.DialogId != Guid.Empty && request.DialogId != dialogId)
                return BadRequest("Не совпадает [ID] в запросе и теле");
            
            await _dialogService.UpdateDialog(request, dialogId, cancellationToken);
            
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpDelete("{dialogId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDialogAsync(
        Guid dialogId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _dialogService.DeleteDialog(dialogId, cancellationToken);
            
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPost("messages")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AddMessageResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMessageAsync(
        [FromBody] AddMessageRequest request,
        Guid dialogId, 
        CancellationToken cancellationToken)
    {
        try
        {
            var response =  await _dialogService.AddMessage(request, cancellationToken);
            
            if (response == null)
                return NotFound();
            
            return Ok(response);
            
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}