using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Main.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return !Guid.TryParse(value, out var userId) 
            ? throw new UnauthorizedAccessException("UserId не найден в токене.") 
            : userId;
    }
}