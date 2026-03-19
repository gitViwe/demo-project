namespace Blazor.Shared.Extension;

public static class JsRuntimeExtension
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public static ValueTask OpenNewTabAsync(this IJSRuntime jsRuntime, string url, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("open", cancellationToken, url, "_blank");
    
    public static ValueTask ImportJsModuleAsync(this IJSRuntime jsRuntime, string modulePath, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("import", cancellationToken, modulePath);
    
    public static ValueTask<IJSObjectReference> ImportJsModuleReferenceAsync(this IJSRuntime jsRuntime, string modulePath, CancellationToken cancellationToken)
        => jsRuntime.InvokeAsync<IJSObjectReference>("import", cancellationToken, modulePath);

    public static async ValueTask<T?> LocalStorageGetAsync<T>(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
    {
        var jsonString = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", cancellationToken, key);

        return string.IsNullOrWhiteSpace(jsonString)
            ? default
            : JsonSerializer.Deserialize<T>(jsonString, SerializerOptions);
    }
    
    public static ValueTask LocalStorageRemoveAsync(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
    
    public static ValueTask LocalStorageSetAsync<T>(this IJSRuntime jsRuntime, string key, T data, CancellationToken cancellationToken)
    {
        var jsonString = JsonSerializer.Serialize(data, SerializerOptions);
        return jsRuntime.LocalStorageSetAsync(key, jsonString, cancellationToken);
    }
    
    public static ValueTask LocalStorageSetAsync(this IJSRuntime jsRuntime, string key, string data, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, data);
    
    public static async ValueTask<T?> SessionStorageGetAsync<T>(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
    {
        var jsonString = await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", cancellationToken, key);

        return string.IsNullOrWhiteSpace(jsonString)
            ? default
            : JsonSerializer.Deserialize<T>(jsonString, SerializerOptions);
    }
    
    public static async ValueTask<string?> SessionStorageGetAsync(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
        => await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", cancellationToken, key);
    
    public static ValueTask SessionStorageRemoveAsync(this IJSRuntime jsRuntime, string key, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", cancellationToken, key);
    
    public static ValueTask SessionStorageSetAsync<T>(this IJSRuntime jsRuntime, string key, T data, CancellationToken cancellationToken)
    {
        var jsonString = JsonSerializer.Serialize(data, SerializerOptions);
        return jsRuntime.SessionStorageSetAsync(key, jsonString, cancellationToken);
    }
    
    public static ValueTask SessionStorageSetAsync(this IJSRuntime jsRuntime, string key, string data, CancellationToken cancellationToken)
        => jsRuntime.InvokeVoidAsync("sessionStorage.setItem", cancellationToken, key, data);
    
    public static ValueTask InitializeTelemetryAsync(this IJSRuntime jsRuntime, TelemetryConfig config, CancellationToken cancellationToken = default)
        => jsRuntime.InvokeVoidAsync("HubOpenTelemetry.Initialize", cancellationToken, config);

    public static ValueTask<TraceContextResponse> TrackEventAsync(this IJSRuntime jsRuntime, SpanRequest request, CancellationToken cancellationToken = default)
        => jsRuntime.InvokeAsync<TraceContextResponse>("HubOpenTelemetry.TrackEvent", cancellationToken, request);

    public static ValueTask<TraceContextResponse> TrackExceptionAsync(this IJSRuntime jsRuntime, SpanRequest request, CancellationToken cancellationToken = default)
        => jsRuntime.InvokeAsync<TraceContextResponse>("HubOpenTelemetry.TrackException", cancellationToken, request);

    public static ValueTask UpdateMetricAsync(this IJSRuntime jsRuntime, MetricRequest request, CancellationToken cancellationToken = default)
        => jsRuntime.InvokeVoidAsync("HubOpenTelemetry.UpdateMetric", cancellationToken, request);
}