namespace Blazor.Infrastructure.Authentication;

internal class AuthenticationHeaderMessageHandler(
    IJSRuntime jsRuntime,
    HubAuthenticationStateProvider stateProvider,
    ILogger<AuthenticationHeaderMessageHandler> logger): DelegatingHandler
{
    private const string HubOpenTelemetryTraceContextKey = "OpenTelemetry.TraceContext";
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await AddAuthenticationHeaderAsync(request, cancellationToken);
        await AddPropagationHeadersAsync(request, cancellationToken);
        
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            await HandleTokenExpiredAsync(response, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unexpected error occurred."),
                ReasonPhrase = "Internal Server Error"
            };
        }
    }
    
    private async Task AddAuthenticationHeaderAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            var claims = await stateProvider.GetAuthenticationStateUserAsync();
            
            if (claims.HasExpiredClaims(1))
            {
                // TODO: implement refresh on auth api
            }
            
            var savedToken = await jsRuntime.SessionStorageGetAsync(HubStorageKey.Identity.AuthToken, cancellationToken);

            if (false == string.IsNullOrWhiteSpace(savedToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            }
        }
    }
    
    private async Task AddPropagationHeadersAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = await jsRuntime.SessionStorageGetAsync<TraceContextResponse>(HubOpenTelemetryTraceContextKey, cancellationToken);

        if (context is not null)
        {
            request.Headers.Add(nameof(context.TraceContext.TraceState), context.TraceContext.TraceState);
            request.Headers.Add(nameof(context.TraceContext.TraceParent), context.TraceContext.TraceParent);
        }
    }

    private async Task HandleTokenExpiredAsync(HttpResponseMessage message, CancellationToken cancellationToken)
    {
        if (message.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Buffer the content so it can be read multiple times
            await message.Content.LoadIntoBufferAsync(cancellationToken);
            
            var response = await message.ToResponseAsync<ProblemDetail>(token: cancellationToken);

            if (response is { Detail: "Token expired" })
            {
                await stateProvider.MarkUserAsLoggedOutAsync();
            }
        }
    }
}