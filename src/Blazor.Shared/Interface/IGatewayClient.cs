namespace Blazor.Shared.Interface;

public interface IGatewayClient
{
    public HttpClient HttpClient { get; }
}