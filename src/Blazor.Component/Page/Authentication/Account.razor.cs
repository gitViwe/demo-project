namespace Blazor.Component.Page.Authentication;

public partial class Account : ComponentBase, IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts => new();
    
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    [Inject]
    public required IAuthenticationManager AuthenticationManager { get; set; }

    public UpdateUserRequest UpdateUserModel { get; set; } = new();
    private bool IsProcessing { get; set; }
    private string IsProcessingMessage { get; set; } = "Processing";
    private string? UserName { get; set; }
    private string? Email { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsProcessingMessage = "Processing";
        IsProcessing = true;
        
        var response = await AuthenticationManager.GetUserDetailAsync(Cts.Token);
        
        // display response messages
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);

        if (response is { Succeeded: true, Data: not null })
        {
            UserName = response.Data.Username;
            Email = response.Data.Email;
            UpdateUserModel.FirstName = response.Data.FirstName;
            UpdateUserModel.LastName = response.Data.LastName;
        }
        
        IsProcessing = false;
    }
    
    private async Task SubmitAsync()
    {
        IsProcessing = true;

        var response = await AuthenticationManager.UpdateDetailsAsync(UpdateUserModel, Cts.Token);
        
        // display response messages
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Warning);

        IsProcessing = false;
    }
    
    private void OnLoadingTimeoutElapsed()
    {
        IsProcessingMessage = "Taking longer than expected";
        Snackbar.Add("We seem to be experiencing some connection issues.", Severity.Warning);
    }
    
    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}