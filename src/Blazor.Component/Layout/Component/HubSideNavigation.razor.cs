namespace Blazor.Component.Layout.Component;

public partial class HubSideNavigation : ComponentBase, IComponentCancellationTokenSource
{
    [Inject]
    public required IScrollManager ScrollManager { get; init; }
    
    public CancellationTokenSource Cts => new();
    
    [CascadingParameter]
    public required CascadingHubFeatureManagerContext HubFeatureManagerContext { get; init; }

    [Inject]
    public required IWebAssemblyHostEnvironment Environment { get; init; }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}