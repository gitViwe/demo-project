import * as opentelemetry from '@opentelemetry/api';
import { resourceFromAttributes } from "@opentelemetry/resources";
import { WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { BatchSpanProcessor } from "@opentelemetry/sdk-trace-base";
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { MeterProvider, PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import {
    getDeviceName,
    getStorageData,
    getDeviceMemory,
    getOnlineStatus,
    getDevicePosture,
    getPreferredLanguages,
} from "./telemetry.util";
import {
    ATTR_SERVICE_NAME,
    ATTR_SERVICE_VERSION
} from "@opentelemetry/semantic-conventions";

interface TelemetryConfig {
    serviceName: string;
    serviceVersion: string;
    gatewayApiUri: string;
}

const initializeTelemetry = (config: TelemetryConfig) => {
    const resource = resourceFromAttributes({
        [ATTR_SERVICE_NAME]: config.serviceName,
        [ATTR_SERVICE_VERSION]: config.serviceVersion,
        ['hub_open_telemetry.blazor.device_name']: getDeviceName()
    });
    
    const traceProvider = new WebTracerProvider({
        resource,
        spanProcessors: [
            new BatchSpanProcessor(
                new OTLPTraceExporter({ url: `${config.gatewayApiUri}/opentelemetry/v1/traces` }),
                {
                    maxExportBatchSize: 100,
                    scheduledDelayMillis: 1000 * 60,
                })
        ]
    });
    traceProvider.register();
    registerInstrumentations({
        instrumentations: [
            new FetchInstrumentation({
                propagateTraceHeaderCorsUrls: [
                    new RegExp(`${config.gatewayApiUri}.*`),
                ],
                clearTimingResources: true,
            }),
        ],
    });
    
    const meterProvider = new MeterProvider({
        resource,
        readers: [new PeriodicExportingMetricReader({
            exporter: new OTLPMetricExporter({ url: `${config.gatewayApiUri}/opentelemetry/v1/metrics` }),
            exportIntervalMillis: 1000 * 60,
            exportTimeoutMillis: 10000,
        })],
    });

    opentelemetry.metrics.setGlobalMeterProvider(meterProvider);
};

export { TelemetryConfig, initializeTelemetry }