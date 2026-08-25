namespace WindowsDeviceBridge.Models;

public sealed record DeviceDto(
    string Id,
    string Name,
    string? Description,
    string? Manufacturer
);
