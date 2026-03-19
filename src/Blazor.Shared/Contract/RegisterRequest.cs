namespace Blazor.Shared.Contract;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }  = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string PasswordConfirmation { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}