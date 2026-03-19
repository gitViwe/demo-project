namespace Blazor.Component.Manager.FeatureManager;

public partial class CascadingHubFeatureManager
    : ComponentBase, IComponentCancellationTokenSource
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(5));
    private CascadingHubFeatureManagerContext _context = new();
    
    public CancellationTokenSource Cts => new();
    
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }
    
    [Inject]
    public required IGatewayClient GatewayClient { get; init; }
    
    [Inject]
    public required IConfiguration Configuration { get; init; }
    
    [Inject]
    public required ILogger<CascadingHubFeatureManager> Logger { get; init; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // initialize feature flags
            await RefreshFeatureFlagsAsync(Cts.Token);
            
            // refresh feature flags indefinitely
            while (await _timer.WaitForNextTickAsync(Cts.Token))
            {
                await RefreshFeatureFlagsAsync(Cts.Token);
            }
        }
    }

    private async Task RefreshFeatureFlagsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await GatewayClient.HttpClient.GetAsync("/feature/feature-flags", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var featureResponse = await response.Content.ReadFromJsonAsync<CascadingHubFeatureManagerContext>(cancellationToken);

                _context = featureResponse ?? new CascadingHubFeatureManagerContext();
                
                StateHasChanged();
            }
        }
        catch (Exception exception)
        {
            Logger.LogWarning(exception, "An unexpected error occurred while getting feature flags.");
        }
    }
    
    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}