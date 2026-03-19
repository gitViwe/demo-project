namespace Blazor.Shared.Interface;

public interface IComponentCancellationTokenSource: IDisposable
{
    public CancellationTokenSource Cts { get; }
}