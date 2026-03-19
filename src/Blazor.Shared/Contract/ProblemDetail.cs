using gitViwe.Shared.ProblemDetail;

namespace Blazor.Shared.Contract;

public sealed class ProblemDetail : IValidationProblemDetails
{
    public string? TraceId { get; set; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public IDictionary<string, object?> Extensions { get; set; }
    public IDictionary<string, string[]> Errors { get; set; }
}