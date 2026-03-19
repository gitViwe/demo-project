namespace Blazor.Component.Page.Authentication;

public partial class RegisterForm
    : ComponentBase, IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts => new();
    
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    [Inject]
    public required IAuthenticationManager AuthenticationManager { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    public RegisterRequest RegisterRequest { get; set; } = new()
    {
        Email = "example@email.com",
        Password = "Password",
        PasswordConfirmation = "Password"
    };
    private bool IsProcessing { get; set; }
    private string IsProcessingMessage { get; set; } = "Processing";
    private static string LoginRoute => HubBlazorPage.Authentication.Login;

    private async Task SubmitAsync()
    {
        IsProcessingMessage = "Processing";
        IsProcessing = true;

        var response = await AuthenticationManager.RegisterAsync(RegisterRequest, Cts.Token);

        // display response messages
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);

        if (response.Succeeded)
        {
            NavigationManager.NavigateTo(HubBlazorPage.Authentication.Account);
        }

        IsProcessing = false;
    }

    private void TrimWhitespaceFromUsername()
    {
        RegisterRequest.UserName = RegisterRequest.UserName.Trim();
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