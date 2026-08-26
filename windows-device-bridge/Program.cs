using WindowsDeviceBridge.Devices;

var reader = new WpdDeviceReader();

var devices = reader.GetDevices();
var fujiPath = @"\External Memory";
var fujiFolder = "DCIM";
var fujiCameraName = "100_FUJI";

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

    var dirEntries = reader.GetRootEntries(device.Id);

    foreach (var dirEntry in dirEntries)
    {
        Console.WriteLine($"{dirEntry.Path} | {dirEntry.Name} | {dirEntry.IsDirectory}");
    }

    var entries = reader.GetEntries(device.Id, fujiPath + @"\" + fujiFolder + @"\" + fujiCameraName);

    var jpegOnly = entries.Where(entry => (
        entry.Name.EndsWith("JPEG", StringComparison.OrdinalIgnoreCase) ||
         entry.Name.EndsWith("JPG", StringComparison.OrdinalIgnoreCase)
    ));

    Console.WriteLine($"{jpegOnly.Count()}");

    foreach (var entry in jpegOnly)
    {
        Console.WriteLine($"{entry.Path} | {entry.Name} | {entry.IsDirectory}");
    }

    var firstPhoto = jpegOnly.First();

    using var stream = reader.ReadFile(device.Id, firstPhoto.Path);

    Console.WriteLine($"{stream.Length} => bytes loaded");
}
