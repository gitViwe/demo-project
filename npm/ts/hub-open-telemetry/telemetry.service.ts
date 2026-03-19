import * as api from "@opentelemetry/api";
import {
    getDevicePosture,
    getDeviceMemory,
    getOnlineStatus,
    getPreferredLanguages,
    getStorageData
} from "./telemetry.util";

const counterCache = new Map<string, api.Counter>();

const getTracer = (appName: string = 'blazor-wasm-client') => api.trace.getTracer(appName);

const getMeter = (appName: string = 'blazor-wasm-client') => api.metrics.getMeter(appName);

const recordMetric = (request: MetricRequest): void => {
    const meter = getMeter();
    let counter = counterCache.get(request.name);

    if (!counter) {
        counter = meter.createCounter(request.name);
        counterCache.set(request.name, counter);
    }

    counter.add(request.value, request.attributes ?? {});
};

const trackSpan = async (request: SpanRequest): Promise<TraceContextResponse> => {
    const tracer = getTracer();

    const spanOptions: api.SpanOptions = {
        kind: request.kind,
        attributes: {
            ['hub_open_telemetry.blazor.device_posture']: getDevicePosture(),
            ['hub_open_telemetry.blazor.device_memory']: getDeviceMemory(),
            ['hub_open_telemetry.blazor.device_online_status']: getOnlineStatus(),
            ['hub_open_telemetry.blazor.device_preferred_languages']: getPreferredLanguages(),
            ['hub_open_telemetry.blazor.device_storage']: await getStorageData()
        },
    };

    if (request.parentContext) {
        spanOptions.links = [{ context: request.parentContext }];
    }

    const span = tracer.startSpan(request.name, spanOptions);

    try {
        if (request.exception) {
            span.recordException(request.exception);
            span.setStatus({ code: api.SpanStatusCode.ERROR });
        } else if (request.eventName) {
            span.addEvent(request.eventName, request.eventAttributes ?? {});
            span.setStatus(request.status || { code: api.SpanStatusCode.OK });
        }

        return extractContextOutput(span);
    } finally {
        span.end();
    }
};

const extractContextOutput = (span: api.Span): TraceContextResponse => {
    const spanCtx = span.spanContext();
    const carrier: any = {};

    api.propagation.inject(
        api.trace.setSpan(api.context.active(), span),
        carrier,
        api.defaultTextMapSetter
    );

    return {
        spanContext: spanCtx,
        traceContext: {
            traceparent: carrier.traceparent || `00-${spanCtx.traceId}-${spanCtx.spanId}-${String(spanCtx.traceFlags).padStart(2, '0')}`,
            tracestate: carrier.tracestate || ''
        }
    };
};

interface MetricRequest {
    name: string;
    value: number;
    attributes?: api.Attributes;
}

interface SpanRequest {
    name: string;
    kind: api.SpanKind;
    parentContext?: api.SpanContext;
    eventName?: string;
    eventAttributes?: api.Attributes;
    exception?: api.Exception;
    status?: api.SpanStatus;
}

interface TraceContextResponse {
    spanContext: api.SpanContext;
    traceContext: { traceparent: string; tracestate: string };
}

export { recordMetric, trackSpan, SpanRequest, TraceContextResponse, MetricRequest }