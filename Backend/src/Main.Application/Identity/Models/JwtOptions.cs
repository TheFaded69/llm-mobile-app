namespace Main.Application.Identity.Models;

public class JwtOptions
{
    public string Issuer { get; set; } = "identity-api";
    public string Audience { get; set; } = "llm-mobile-app";
    public string Key { get; set; } = "7r6zCoTl7EF6Sj3FnuMRUmbp0LfSMIlT";
    public int AccessTokenMinutes { get; set; } = 30;
    public int RefreshTokenDays { get; set; } = 30;
}
