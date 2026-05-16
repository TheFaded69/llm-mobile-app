namespace Main.Application.Identity.Services;

public interface IExternalIdentityValidator
{
    Task<ExternalIdentityValidationResult> ValidateGoogleAsync(string idToken, CancellationToken cancellationToken);
    Task<ExternalIdentityValidationResult> ValidateAppleAsync(string identityToken, CancellationToken cancellationToken);
}

public class ExternalIdentityValidationResult
{
    public required string ProviderUserId { get; init; }
    public string? Email { get; init; }
    public string? DisplayName { get; init; }
}
