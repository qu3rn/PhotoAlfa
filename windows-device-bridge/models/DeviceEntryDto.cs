namespace WindowsDeviceBridge.Models;

public sealed record DeviceEntryDto(
    string Path,
    string Name,
    bool IsDirectory,
    ulong? Size,
    DateTimeOffset? ModifiedAt
);
