namespace Blazor.Component.Section.Home;

public partial class HeroSection : ComponentBase, IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts { get; } = new();

    [Inject]
    public required IJSRuntime Runtime { get; init; }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}