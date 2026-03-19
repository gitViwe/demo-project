namespace Blazor.Infrastructure.Client;

internal class GatewayClient(HttpClient client) : IGatewayClient
{
    public const string ResilienceHandlerName = "GatewayClientResilienceHandler";
    public HttpClient HttpClient => client;
}