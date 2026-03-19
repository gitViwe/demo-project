namespace Blazor.Component.WebAuthn;

public static class Startup
{
    public static IServiceCollection AddHubWebAuthenticationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IHubWebAuthenticationManager, HubWebAuthenticationManager>();
    }
}