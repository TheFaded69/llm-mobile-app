namespace Identity.Contracts.V1.Responses;

public class AuthTokensResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTimeOffset AccessTokenExpiresAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}
