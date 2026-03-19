import { initializeTelemetry, TelemetryConfig } from "./hub-open-telemetry/telemetry.config";
import {
    trackSpan,
    recordMetric,
    SpanRequest,
    MetricRequest,
    TraceContextResponse
} from "./hub-open-telemetry/telemetry.service";
import {
    AuthenticatorAttestationRawResponse,
    AuthenticatorAssertionRawResponse,
    isWebAuthnPossible,
    createCredentials,
    verify
} from "./hub-web-authentication/hub-web-authentication";

declare global {
    // extend Window to add a custom property
    interface Window {
        HubComponent: HubComponent;
        HubWebAuthentication: HubWebAuthentication;
        HubOpenTelemetry: HubOpenTelemetry;
    }

    interface HubComponent {}

    interface HubWebAuthentication {
        IsWebAuthnPossible(): boolean,
        CreateCredentials(options: PublicKeyCredentialCreationOptions): Promise<AuthenticatorAttestationRawResponse>,
        Verify(options: PublicKeyCredentialRequestOptions): Promise<AuthenticatorAssertionRawResponse>
    }
    
    interface HubOpenTelemetry {
        Initialize(config: TelemetryConfig): void;
        TrackEvent(request: SpanRequest): Promise<TraceContextResponse>;
        TrackException(request: SpanRequest): Promise<TraceContextResponse>;
        UpdateMetric(request: MetricRequest): void;
    }
}

// assign functions
window.HubComponent = {};

window.HubWebAuthentication = {
    IsWebAuthnPossible: isWebAuthnPossible,
    CreateCredentials: createCredentials,
    Verify: verify,
};

window.HubOpenTelemetry = {
    Initialize: initializeTelemetry,
    TrackEvent: trackSpan,
    TrackException: trackSpan,
    UpdateMetric: recordMetric
}