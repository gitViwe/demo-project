namespace Blazor.Component.WebAuthn.Serializer;

[JsonSerializable(typeof(AssertionOptions))]
[JsonSerializable(typeof(AuthenticatorAssertionRawResponse))]
[JsonSerializable(typeof(AuthenticatorAttestationRawResponse))]
[JsonSerializable(typeof(HubAuthenticatorAttestationRawResponse))]
[JsonSerializable(typeof(CredentialCreateOptions))]
public partial class FidoBlazorSerializerContext : JsonSerializerContext;