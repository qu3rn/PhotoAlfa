using MediaDevices;
using WindowsDeviceBridge.Models;

namespace WindowsDeviceBridge.Devices;

public sealed class WpdDeviceReader : IDeviceReader
{
    public IReadOnlyList<DeviceDto> GetDevices()
    {
        var devices = MediaDeviceManager
            .Instance
            .GetDevices()?
            .ToList() ?? [];

        return devices
        .Select(device => new DeviceDto(
            device.DeviceId,
            device.FriendlyName ?? "Unknown device",
            device.Description,
            device.Manufacturer
        )).ToList();
    }

    public IReadOnlyList<DeviceEntryDto> GetRootEntries(string deviceId)
    {
        var device = MediaDeviceManager
        .Instance
        .GetDevices()?
        .First(d => d.DeviceId == deviceId);

        if (device is null)
        {
            return [];
        }

        device.Connect();

        var drives = device.GetDrives().ToList();

        return drives.Select(d => new DeviceEntryDto(
            d?.Name ?? "",
            d?.RootDirectory?.FullName ?? "",
            true
        )).ToList();
    }
}
