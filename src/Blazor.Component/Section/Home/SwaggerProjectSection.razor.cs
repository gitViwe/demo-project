namespace Blazor.Component.Section.Home;

public partial class SwaggerProjectSection : ComponentBase
{
    [CascadingParameter]
    public required CascadingHubFeatureManagerContext HubFeatureManagerContext { get; init; }
    
    [Inject]
    public required IConfiguration Configuration { get; init; }
}