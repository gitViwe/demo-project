namespace Blazor.Component;

public static class Startup
{
    public static IServiceCollection AddHubComponentServices(this IServiceCollection services)
    {
        return services.AddMudServices();
    }
}