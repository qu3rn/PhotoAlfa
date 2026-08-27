namespace WindowsDeviceBridge.Contracts;

public sealed record BridgeResponse<T>(
    bool Success,
    T? Data = default,
    string? Error = null
);

public sealed record ImageResponse(
    string MimeType,
    string Base64
);
