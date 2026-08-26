using WindowsDeviceBridge.Models;

namespace WindowsDeviceBridge.Devices;

public interface IDeviceReader
{
    IReadOnlyList<DeviceDto> GetDevices();
    IReadOnlyList<DeviceEntryDto> GetRootEntries(string deviceId);
    IReadOnlyList<DeviceEntryDto> GetEntries(string deviceId, string path);

    Stream ReadFile(string deviceId, string path);
    string ReadFileAsBase64(string deviceId, string path);
}
