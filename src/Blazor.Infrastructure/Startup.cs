using Microsoft.Extensions.Configuration;

namespace Blazor.Infrastructure;

public static class Startup
{
    public static IServiceCollection RegisterClientAuthorization(this IServiceCollection services)
    {
        services
            .AddOptions()
            .AddAuthorizationCore()
            .AddScoped<HubAuthenticationStateProvider>()
            .AddScoped<IAuthenticationManager, HubAuthenticationManager>()
            .AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<HubAuthenticationStateProvider>());

        return services;
    }
    
    public static IServiceCollection AddGatewayClient(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<AuthenticationHeaderMessageHandler>()
            .AddHttpClient<IGatewayClient, GatewayClient>(options =>
            {
                options.Timeout = TimeSpan.FromSeconds(60);
                options.BaseAddress = new Uri(configuration["BlazorConfiguration:Uri:GatewayApi"]!);
            })
            .AddHttpMessageHandler<AuthenticationHeaderMessageHandler>()
            .AddResilienceHandler(GatewayClient.ResilienceHandlerName, resilienceBuilder =>
            {
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    Delay = TimeSpan.FromSeconds(5),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .Handle<TaskCanceledException>()
                        .HandleResult(response => response.StatusCode == HttpStatusCode.GatewayTimeout)
                });
            });
        
        return services;
    }
}