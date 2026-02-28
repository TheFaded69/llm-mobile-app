namespace Identity.Application.Models;

public class JwtOptions
{
    public string Issuer { get; set; } = "identity-api";
    public string Audience { get; set; } = "llm-mobile-app";
    public string Key { get; set; } = "CHANGE_ME_FOR_PRODUCTION_MIN_32_CHARS";
    public int AccessTokenMinutes { get; set; } = 30;
    public int RefreshTokenDays { get; set; } = 30;
}
