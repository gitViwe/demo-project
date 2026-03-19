namespace Blazor.Shared.Constant;

public static class HubBlazorPage
{
    public static class Playground
    {
        public const string Section = "playground-section";
        public const string Chore = "chore";
        public const string FetchData = "fetchdata";
    }
    
    public static class Documentation
    {
        public const string Swagger = "documentation-swagger";
        public const string Scalar = "documentation-scalar";
        public const string GraphQl = "documentation-graphql";
    }
    
    public static class Observability
    {
        public const string OpenTelemetry = "observability-open-telemetry";
        public const string HealthCheck = "observability-health-check";
    }
    
    public static class Security
    {
        public const string WebAuthn = "security-webauthn";
        public const string Mfa = "security-mfa";
    }
    
    public static class Communication
    {
        public const string SignalR = "paradigm-signalr";
        public const string Grpc = "paradigm-grpc";
    }
    
    public static class Authentication
    {
        public const string Login = "authentication-login";
        public const string Register = "authentication-register";
        public const string Account = "authentication-account";
    }
}