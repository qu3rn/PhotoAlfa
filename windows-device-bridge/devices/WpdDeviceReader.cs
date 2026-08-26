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
        .FirstOrDefault(d => d.DeviceId == deviceId);

        if (device is null)
        {
            return [];
        }

        device.Connect();

        var drives = device.GetDrives().ToList();

        var entries = drives.Select(d => new DeviceEntryDto(
            d?.Name ?? "",
            d?.RootDirectory?.FullName ?? "",
            true,
            null,
            null
        )).ToList();

        device.Disconnect();

        return entries;
    }

    public IReadOnlyList<DeviceEntryDto> GetEntries(string deviceId, string path)
    {
        var device = MediaDeviceManager
        .Instance
        .GetDevices()?
        .FirstOrDefault(d => d.DeviceId == deviceId);

        if (device is null)
        {
            return [];
        }

        device.Connect();

        try
        {
            var directories = device
            .GetDirectories(path)
            .Select(dirPath => new DeviceEntryDto(
                dirPath,
                Path.GetFileName(dirPath),
                true,
                null,
                null
            ));

            var files = device
            .GetFiles(path)
            .Select(filePath => new DeviceEntryDto(
                filePath,
                Path.GetFileName(filePath),
                false,
                null,
                null
            ));

            return directories
            .Concat(files)
            .ToList();
        }
        finally
        {
            device.Disconnect();
        }
    }

    public Stream ReadFile(string deviceId, string path)
    {
        var device = MediaDeviceManager
         .Instance
         .GetDevices()?
         .FirstOrDefault(d => d.DeviceId == deviceId);

        if (device is null)
        {
            throw new InvalidOperationException("No device found");
        }

        device.Connect();

        try
        {
            var stream = new MemoryStream();

            device.DownloadFile(path, stream);

            stream.Position = 0;

            return stream;
        }
        finally
        {
            device.Disconnect();
        }
    }

    public string ReadFileAsBase64(string deviceId, string path)
    {
        using var stream = ReadFile(deviceId, path);

        var bytes = ((MemoryStream)stream).ToArray();

        var base64 = Convert.ToBase64String(bytes);

        return base64;
    }
}
