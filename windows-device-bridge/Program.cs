using WindowsDeviceBridge.Devices;

var reader = new WpdDeviceReader();

var devices = reader.GetDevices();

if (devices.Count == 0)
{
    Console.WriteLine("No devices found");
    return;
}

foreach (var device in devices)
{
    Console.WriteLine(
        $"{device.Name} | {device.Manufacturer} | {device.Id}"
    );

    var entries = reader.GetRootEntries(device.Id);

    foreach (var entry in entries)
    {
        Console.WriteLine($"{entry.Path} | {entry.Name} | {entry.IsDirectory}");
    }
}
