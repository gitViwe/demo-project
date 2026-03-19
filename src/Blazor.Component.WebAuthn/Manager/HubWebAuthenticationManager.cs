namespace Blazor.Component.WebAuthn.Manager;

internal sealed class HubWebAuthenticationManager(
    IJSRuntime jsRuntime,
    IGatewayClient gatewayClient,
    IConfiguration configuration,
    ILogger<HubWebAuthenticationManager> logger) : IHubWebAuthenticationManager
{
    private static readonly JsonSerializerOptions JsonOptions = new Serializer.FidoBlazorSerializerContext().Options;
    
    public ValueTask<bool> IsWebAuthnSupportedAsync(CancellationToken cancellationToken)
        => jsRuntime.InvokeAsync<bool>("HubWebAuthentication.IsWebAuthnPossible", cancellationToken);
    
    public async Task<IResponse> RegisterAsync(
        string displayName,
        CancellationToken cancellationToken)
    {
        // Get options from server
        var credentialOptionsResponse = await gatewayClient.HttpClient.GetAsync(configuration.GetWebAuthnCredentialOptionsUri(displayName), cancellationToken);

        if (credentialOptionsResponse.IsSuccessStatusCode is false)
        {
            return Response.Fail("No options received");
        }
        
        var options = await credentialOptionsResponse.Content.ReadFromJsonAsync<CredentialCreateOptions>(JsonOptions, cancellationToken);

        if (options is null)
        {
            return Response.Fail("No options received");
        }

        try
        {
            // Present options to user and get response
            var credential = await jsRuntime.InvokeAsync<HubAuthenticatorAttestationRawResponse>("HubWebAuthentication.CreateCredentials", cancellationToken, options);
            // Send response to server
            var createCredentialsResponse = await gatewayClient.HttpClient.PutAsJsonAsync(configuration.GetWebAuthnCreateCredentialsUri(options.User.Name), credential, JsonOptions, cancellationToken);
            
            return createCredentialsResponse.IsSuccessStatusCode
                ? Response.Success("Successfully created credentials")
                : Response.Fail("Failed to create credentials");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to create credentials");
            var message = "Failed to create credentials." + (options.ExcludeCredentials.Count > 0 ? " (You may have already registered this device)" : string.Empty);
            return Response.Fail(message);
        }
    }

    public async Task<IResponse> LoginAsync(CancellationToken cancellationToken)
    {
        var assertionOptionsResponse = await gatewayClient.HttpClient.GetAsync(configuration.GetWebAuthnAssertionOptionsUri(), cancellationToken);

        if (assertionOptionsResponse.IsSuccessStatusCode is false)
        {
            return Response.Fail("No options received");
        }
        
        var options = await assertionOptionsResponse.Content.ReadFromJsonAsync<AssertionOptions>(JsonOptions, cancellationToken);

        if (options is null)
        {
            return Response.Fail("No options received");
        }
        
        try
        {
            // Present options to user and get response (usernameless users will be asked by their authenticator, which credential they want to use to sign the challenge)
            var assertion = await jsRuntime.InvokeAsync<AuthenticatorAssertionRawResponse>("HubWebAuthentication.Verify", cancellationToken, options);
            
            // Send response to server
            var assertionResponse = await gatewayClient.HttpClient.PostAsJsonAsync(configuration.GetWebAuthnAssertionUri(), assertion, JsonOptions, cancellationToken);

            return assertionResponse.IsSuccessStatusCode
                ? Response.Success("Successfully created token")
                : Response.Fail("Failed to create token");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to create token");
            return Response.Fail("Failed to create token");
        }
    }
}