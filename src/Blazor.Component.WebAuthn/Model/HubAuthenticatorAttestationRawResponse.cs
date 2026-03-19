namespace Blazor.Component.WebAuthn.Model;

public sealed class HubAuthenticatorAttestationRawResponse
{
    [JsonConverter(typeof(Base64UrlConverter))]
    [JsonPropertyName("id")]
    public required byte[] Id { get; init; }

    [JsonConverter(typeof(Base64UrlConverter))]
    [JsonPropertyName("rawId")]
    public required byte[] RawId { get; init; }

    [JsonPropertyName("type")]
    public PublicKeyCredentialType? Type { get; init; }

    [JsonPropertyName("response")]
    public required AuthenticatorAttestationRawResponse.AttestationResponse Response { get; init; }

    [JsonPropertyName("extensions")]
    public required AuthenticationExtensionsClientOutputs Extensions { get; init; }
}