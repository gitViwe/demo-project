interface AuthenticatorAttestationRawResponse {
    id: string;
    rawId: string;
    type?: string;
    response: ResponseData;
    extensions: AuthenticationExtensionsClientOutputs;
}

interface ResponseData {
    attestationObject: string;
    clientDataJSON: string;
    transports: string[];
}

interface AuthenticatorAssertionRawResponse {
    id: string;
    rawId: string;
    response: AssertionResponse;
    type?: string;
    extensions: AuthenticationExtensionsClientOutputs;
}

interface AssertionResponse {
    authenticatorData: string;
    signature: string;
    clientDataJSON: string;
    userHandle?: string;
}

function isWebAuthnPossible() {
    return !!window.PublicKeyCredential;
}

function toBase64Url(arrayBuffer: ArrayBuffer): string {
    return btoa(String.fromCharCode(...new Uint8Array(arrayBuffer))).replace(/\+/g, "-").replace(/\//g, "_").replace(/=*$/g, "");
}
function fromBase64Url(value: string): ArrayBuffer {
    return Uint8Array.from(atob(value.replace(/-/g, "+").replace(/_/g, "/")), c => c.charCodeAt(0)).buffer;
}
function base64StringToUrl(base64String: string): string {
    return base64String.replace(/\+/g, "-").replace(/\//g, "_").replace(/=*$/g, "");
}

async function createCredentials(options: PublicKeyCredentialCreationOptions): Promise<AuthenticatorAttestationRawResponse> {
    if (typeof options.challenge === 'string')
        options.challenge = fromBase64Url(options.challenge);
    if (typeof options.user.id === 'string')
        options.user.id = fromBase64Url(options.user.id);
    if (options.rp.id === null)
        options.rp.id = undefined;
    if (options.excludeCredentials) {
        for (let cred of options.excludeCredentials) {
            if (typeof cred.id === 'string')
                cred.id = fromBase64Url(cred.id);
        }
    }
    const newCredentials = await navigator.credentials.create({publicKey: options}) as PublicKeyCredential;
    const response = newCredentials.response as AuthenticatorAttestationResponse;

    return {
        id: base64StringToUrl(newCredentials.id),
        rawId: toBase64Url(newCredentials.rawId),
        type: newCredentials.type,
        extensions: newCredentials.getClientExtensionResults(),
        response: {
            attestationObject: toBase64Url(response.attestationObject),
            clientDataJSON: toBase64Url(response.clientDataJSON),
            transports: response.getTransports ? response.getTransports() : []
        }
    };
}

async function verify(options: PublicKeyCredentialRequestOptions): Promise<AuthenticatorAssertionRawResponse> {
    if (typeof options.challenge === 'string')
        options.challenge = fromBase64Url(options.challenge);
    if (options.allowCredentials) {
        for (let i = 0; i < options.allowCredentials.length; i++) {
            const id = options.allowCredentials[i].id;
            if (typeof id === 'string')
                options.allowCredentials[i].id = fromBase64Url(id);
        }
    }
    const credentials = await navigator.credentials.get({publicKey: options}) as PublicKeyCredential;
    const response = credentials.response as AuthenticatorAssertionResponse;
    return {
        id: credentials.id,
        rawId: toBase64Url(credentials.rawId),
        type: credentials.type,
        extensions: credentials.getClientExtensionResults(),
        response: {
            authenticatorData: toBase64Url(response.authenticatorData),
            clientDataJSON: toBase64Url(response.clientDataJSON),
            userHandle: response.userHandle && response.userHandle.byteLength > 0 ? toBase64Url(response.userHandle) : undefined,
            signature: toBase64Url(response.signature)
        }
    };
}

export { AuthenticatorAttestationRawResponse, AuthenticatorAssertionRawResponse, isWebAuthnPossible, createCredentials, verify };