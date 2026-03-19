namespace Blazor.Component.Manager.FeatureManager;

public sealed class CascadingHubFeatureManagerContext
{
    public IEnumerable<string> Features { get; init; } = [];
    public bool IsFeatureEnabled(string feature) => !string.IsNullOrWhiteSpace(feature) && Features.Contains(feature);
}