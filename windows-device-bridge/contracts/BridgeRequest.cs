namespace WindowsDeviceBridge.Contracts;

public sealed record BrdigeRequest(
    string Command,
    string? DeviceId = null,
    string? Path = null
);
