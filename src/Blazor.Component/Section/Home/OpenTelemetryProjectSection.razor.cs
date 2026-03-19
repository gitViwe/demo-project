namespace Blazor.Component.Section.Home;

public partial class OpenTelemetryProjectSection : ComponentBase
{
    [CascadingParameter]
    public required CascadingHubFeatureManagerContext HubFeatureManagerContext { get; init; }
    
    [Inject]
    public required IConfiguration Configuration { get; init; }
    
    private string ToolTipText
        => string.IsNullOrWhiteSpace(Configuration.GetSeqUiUri())
           || string.IsNullOrWhiteSpace(Configuration.GetJaegerUiUri())
           || string.IsNullOrWhiteSpace(Configuration.GetGrafanaDashboardUri())
            ? "Run via Docker to enable these" : string.Empty;
    private string GrafanaCredentialsToolTipText
        => string.IsNullOrWhiteSpace(ToolTipText) ? "username: admin | password: admin" : string.Empty;
}