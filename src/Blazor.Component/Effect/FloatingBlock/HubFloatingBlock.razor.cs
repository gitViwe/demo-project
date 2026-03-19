namespace Blazor.Component.Effect.FloatingBlock;

public partial class HubFloatingBlock : ComponentBase
{
    private const int MaxCount = 30;
    [Parameter] public int BlockCount { get; set; } = 10;
    private int GetBlockCount()
    {
        return BlockCount <= MaxCount ? BlockCount : MaxCount;
    }
}