using Microsoft.AspNetCore.Mvc;
using Users.Application.Handlers;
using Users.Contracts.V1.Requests;
using Users.Contracts.V1.Responses;

namespace Users.Api.Controllers.V1;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly AddUserHandler _addUserHandler;
    private readonly UpdateUserHandler _updateUserHandler;
    private readonly DeleteUserHandler _deleteUserHandler;
    private readonly GetUserHandler _getUserHandler;

    public UserController(
        AddUserHandler addUserHandler,
        UpdateUserHandler updateUserHandler,
        DeleteUserHandler deleteUserHandler,
        GetUserHandler getUserHandler
        )
    {
        _addUserHandler = addUserHandler;
        _updateUserHandler = updateUserHandler;
        _deleteUserHandler = deleteUserHandler;
        _getUserHandler = getUserHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserAsync(
        [FromBody] AddUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _addUserHandler.Handle(request, cancellationToken);
            
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
    public async Task<IActionResult> UpdateUserAsync(
        Guid id, 
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id != Guid.Empty && request.Id != id)
                return BadRequest("Не совпадает [ID] в запросе и теле");
            
            await _updateUserHandler.Handle(request, cancellationToken);
            
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
    public async Task<IActionResult> DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteUserRequest()
            {
                Id = id
            };
            
            await _deleteUserHandler.Handle(request, cancellationToken);
            
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetUserRequest()
            {
                Id = id
            };
            
            var response = await _getUserHandler.Handle(request, cancellationToken);
            
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