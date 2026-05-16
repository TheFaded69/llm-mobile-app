using System.IdentityModel.Tokens.Jwt;
using Main.Application.Identity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Main.Application.Identity.Services;

public class OidcExternalIdentityValidator : IExternalIdentityValidator
{
    private readonly ExternalAuthOptions _options;
    private readonly IConfigurationManager<OpenIdConnectConfiguration> _googleConfigurationManager;
    private readonly IConfigurationManager<OpenIdConnectConfiguration> _appleConfigurationManager;

    public OidcExternalIdentityValidator(IOptions<ExternalAuthOptions> options)
    {
        _options = options.Value;

        _googleConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "https://accounts.google.com/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever());

        _appleConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "https://appleid.apple.com/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever());
    }

    public Task<ExternalIdentityValidationResult> ValidateGoogleAsync(string idToken, CancellationToken cancellationToken) =>
        ValidateAsync(idToken, _googleConfigurationManager, _options.Google.AllowedAudiences, "https://accounts.google.com", cancellationToken);

    public Task<ExternalIdentityValidationResult> ValidateAppleAsync(string identityToken, CancellationToken cancellationToken) =>
        ValidateAsync(identityToken, _appleConfigurationManager, _options.Apple.AllowedAudiences, "https://appleid.apple.com", cancellationToken);

    private static async Task<ExternalIdentityValidationResult> ValidateAsync(
        string token,
        IConfigurationManager<OpenIdConnectConfiguration> configurationManager,
        List<string> audiences,
        string issuer,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("Токен внешней аутентификации не передан.");

        var configuration = await configurationManager.GetConfigurationAsync(cancellationToken);

        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = configuration.SigningKeys,
            ValidateIssuer = true,
            ValidIssuers = [issuer],
            ValidateAudience = audiences.Count > 0,
            ValidAudiences = audiences,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, parameters, out var validatedToken);

        if (validatedToken is not JwtSecurityToken)
            throw new InvalidOperationException("Некорректный формат JWT токена внешнего провайдера.");

        var providerUserId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                             ?? throw new InvalidOperationException("Внешний провайдер не вернул subject.");

        return new ExternalIdentityValidationResult
        {
            ProviderUserId = providerUserId,
            Email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value,
            DisplayName = principal.FindFirst("name")?.Value
        };
    }
}
