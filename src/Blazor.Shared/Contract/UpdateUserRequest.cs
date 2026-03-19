namespace Blazor.Shared.Contract;

public sealed class UpdateUserRequest
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
}