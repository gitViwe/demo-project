namespace Blazor.Component.Manager.OpenTelemetry;

public sealed class CascadingHubOpenTelemetryManagerContext(IJSRuntime jsRuntime)
{
    private const string HubOpenTelemetryTraceContextKey = "OpenTelemetry.TraceContext";

    private ValueTask PersistTraceContextAsync(TraceContextResponse traceContext, CancellationToken cancellation)
        => jsRuntime.SessionStorageSetAsync(HubOpenTelemetryTraceContextKey, traceContext, cancellation);

    private ValueTask<TraceContextResponse?> GetTraceContextAsync(CancellationToken cancellation)
        => jsRuntime.SessionStorageGetAsync<TraceContextResponse>(HubOpenTelemetryTraceContextKey, cancellation);

    public async Task TrackEventAsync(
        string name,
        string eventName,
        Dictionary<string, object> eventAttributes,
        CancellationToken cancellation = default)
    {
        var context = await GetTraceContextAsync(cancellation);
        var request = SpanRequest.Create(name, eventName, eventAttributes, context?.SpanContext);
        var response = await jsRuntime.TrackEventAsync(request, cancellation);
        await PersistTraceContextAsync(response, cancellation);
    }

    public async Task TrackExceptionAsync(
        string name,
        Exception exception,
        CancellationToken cancellation = default)
    {
        var context = await GetTraceContextAsync(cancellation);
        var request = SpanRequest.Create(name, exception, context?.SpanContext);
        var response = await jsRuntime.TrackExceptionAsync(request, cancellation);
        await PersistTraceContextAsync(response, cancellation);
    }

    public async ValueTask UpdateMetricAsync(MetricRequest request, CancellationToken cancellation = default)
        => await jsRuntime.UpdateMetricAsync(request, cancellation);
}