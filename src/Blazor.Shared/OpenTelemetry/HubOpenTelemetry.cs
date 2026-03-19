namespace Blazor.Shared.OpenTelemetry;

public record TelemetryConfig(
    string ServiceName,
    string ServiceVersion,
    string GatewayApiUri
);

public record MetricRequest(
    string Name,
    double Value,
    Dictionary<string, object>? Attributes = null
);

public sealed class SpanContext
{
    [JsonPropertyName("traceId")]
    public required string TraceId { get; init; }

    [JsonPropertyName("spanId")]
    public required string SpanId { get; init; }

    [JsonPropertyName("isRemote")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsRemote { get; init; }

    [JsonPropertyName("traceFlags")]
    public int TraceFlags { get; init; }

    [JsonPropertyName("traceState")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraceState { get; init; }
}

public record SpanException(
    string Name,
    string Message,
    string? Code = null,
    string? Stack = null
);

public enum SpanKind
{
    Internal = 0,
    Server = 1,
    Client = 2,
    Producer = 3,
    Consumer = 4
}

public sealed class SpanRequest
{
    public required string Name { get; init; }
    
    public SpanKind Kind { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SpanContext? ParentContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EventName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? EventAttributes { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SpanException? Exception { get; init; }

    public static SpanRequest Create(
        string name,
        Exception exception,
        SpanContext? parentContext = null)
        => new SpanRequest()
        {
            Name = name,
            Kind = SpanKind.Internal,
            Exception = new SpanException(
                exception.GetType().Name,
                exception.Message,
                exception.HelpLink,
                exception.StackTrace),
            ParentContext = parentContext,
        };
    
    public static SpanRequest Create(
        string name,
        string eventName,
        Dictionary<string, object> eventAttributes,
        SpanContext? parentContext = null)
        => new SpanRequest()
        {
            Name = name,
            Kind = SpanKind.Internal,
            EventName =  eventName,
            EventAttributes = eventAttributes,
            ParentContext = parentContext,
        };
}

public record TraceContextResponse(
    [property: JsonPropertyName("spanContext")] SpanContext SpanContext,
    [property: JsonPropertyName("traceContext")] TraceContextInfo TraceContext
);

public record TraceContextInfo(
    [property: JsonPropertyName("traceparent")] string TraceParent,
    [property: JsonPropertyName("tracestate")] string TraceState
);