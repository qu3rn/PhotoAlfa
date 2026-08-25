using WindowsDeviceBridge.Models;

namespace WindowsDeviceBridge.Devices;

public interface IDeviceReader
{
    IReadOnlyList<DeviceDto> GetDevices();
    IReadOnlyList<DeviceEntryDto> GetRootEntries(string deviceId);
}
