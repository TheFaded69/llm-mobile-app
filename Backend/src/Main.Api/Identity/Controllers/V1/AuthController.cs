using Main.Application.Identity.Services;
using Main.Contract.Identity.V1.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api.Identity.Controllers.V1;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var tokens = await _authService.RegisterAsync(request.Email, request.Password, request.Username, cancellationToken);
            return Ok(tokens);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var tokens = await _authService.LoginAsync(request.Email, request.Password, cancellationToken);
            return Ok(tokens);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var tokens = await _authService.RefreshAsync(request.RefreshToken, cancellationToken);
            return Ok(tokens);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost("password/forgot")]
    public async Task<IActionResult> Forgot([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.ForgotPasswordAsync(request.Email, cancellationToken);
        return Ok(response);
    }

    [HttpPost("password/reset")]
    public async Task<IActionResult> Reset([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.ResetPasswordAsync(request.ResetToken, request.NewPassword, cancellationToken);
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost("external/google")]
    public async Task<IActionResult> Google([FromBody] ExternalLoginRequest request, CancellationToken cancellationToken)
    {
        var tokens = await _authService.ExternalLoginAsync("google", request.IdToken ?? string.Empty, request.Username, cancellationToken);
        return Ok(tokens);
    }

    [HttpPost("external/apple")]
    public async Task<IActionResult> Apple([FromBody] ExternalLoginRequest request, CancellationToken cancellationToken)
    {
        var tokens = await _authService.ExternalLoginAsync("apple", request.IdentityToken ?? string.Empty, request.Username, cancellationToken);
        return Ok(tokens);
    }
}
