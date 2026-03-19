namespace Blazor.Component.WebAuthn;

public class RegisterRequest
{
    public string DisplayName { get; set; } = string.Empty;
}

public partial class HubWebAuthentication
    : ComponentBase, IComponentCancellationTokenSource
{
    private RegisterRequest RegisterModel = new();
    private bool IsProcessing { get; set; }
    
    public CancellationTokenSource Cts { get; } = new();
    
    [Inject]
    public required ISnackbar Snackbar { get; init; }
    
    [Inject]
    public required IHubWebAuthenticationManager HubWebAuthenticationManager { get; init; }

    private async Task RegisterAsync()
    {
        IsProcessing = true;
        var response = await HubWebAuthenticationManager.RegisterAsync(RegisterModel.DisplayName, Cts.Token);
        
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);
        IsProcessing = false;
    }

    private async Task LoginAsync()
    {
        IsProcessing = true;
        var response = await HubWebAuthenticationManager.LoginAsync(Cts.Token);
        
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);
        IsProcessing = false;
    }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}