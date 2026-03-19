namespace Blazor.Component.WebAuthn;

public interface IHubWebAuthenticationManager
{
    ValueTask<bool> IsWebAuthnSupportedAsync(CancellationToken cancellationToken);
    Task<IResponse> RegisterAsync(string displayName, CancellationToken cancellationToken);
    Task<IResponse> LoginAsync(CancellationToken cancellationToken);
}