namespace Blazor.Component.Manager.FeatureManager;

public partial class HubFeatureManagerComponent
    : ComponentBase, IComponentCancellationTokenSource
{
    private bool _isLoading;
    private bool _featureIsEnabled;
    public CancellationTokenSource Cts { get; } = new();
    
    [CascadingParameter]
    public required CascadingHubFeatureManagerContext HubFeatureManagerContext { get; init; }

    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }

    [Parameter]
    public RenderFragment? LoadingContent { get; init; }

    [Parameter]
    public RenderFragment? DisabledContent { get; init; }

    [Parameter, EditorRequired]
    public required string FeatureName { get; init; }

    protected override void OnInitialized()
    {
        ArgumentNullException.ThrowIfNull(HubFeatureManagerContext, nameof(HubFeatureManagerContext));
        _isLoading = true;
        _featureIsEnabled = HubFeatureManagerContext.IsFeatureEnabled(FeatureName);
        _isLoading = false;
    }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}