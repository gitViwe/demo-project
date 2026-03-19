namespace Blazor.Component.Manager.OpenTelemetry;

public partial class CascadingHubOpenTelemetryManager
    : ComponentBase, IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts => new();
    private CascadingHubOpenTelemetryManagerContext? _context;
    
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }
    
    [Inject]
    public required IConfiguration Configuration { get; init; }
    
    [Inject]
    public required IJSRuntime JsRuntime { get; init; }
    
    [Inject]
    public required ILogger<CascadingHubOpenTelemetryManager> Logger { get; init; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                var config = new TelemetryConfig(
                    ServiceName: Configuration.GetServiceName(),
                    ServiceVersion: Configuration.GetServiceVersion(),
                    GatewayApiUri: Configuration.GetGatewayApiUri()
                );

                await JsRuntime.InitializeTelemetryAsync(config, Cts.Token);
                
                _context = new CascadingHubOpenTelemetryManagerContext(JsRuntime);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to initialize HubOpenTelemetry.");
            }
        }
    }
    
    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}