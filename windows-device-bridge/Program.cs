using System.Text.Json;
using WindowsDeviceBridge.Contracts;
using WindowsDeviceBridge.Devices;

var reader = new WpdDeviceReader();

var fujiPath = @"\\External Memory\DCIM\100_FUJI";

var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

while (true)
{
    var line = Console.ReadLine();

    if (line is null)
    {
        break;
    }

    try
    {
        var request = JsonSerializer.Deserialize<BrdigeRequest>(
            line,
            options
        );

        if (request is null)
        {
            continue;
        }

        object response = request.Command switch
        {
            "devices" => new BridgeResponse<object>(
                true,
                reader.GetDevices()
            ),

            "entries" when
                request.DeviceId is not null &&
                request.Path is not null
                => new BridgeResponse<object>(
                    true,
                    reader.GetEntries(
                        request.DeviceId,
                        request.Path
                        )
                ),

            "read-file" when
                request.DeviceId is not null &&
                request.Path is not null
                => ReadFile(
                    reader,
                    request.DeviceId,
                    request.Path
                ),

            _ => new BridgeResponse<object>(
                false,
                Error: $"Unknown command: {request.Command}"
            )
        };

        Console.WriteLine(
            JsonSerializer.Serialize(response, options)
        );
    }
    catch (Exception ex)
    {
        var response = new BridgeResponse<object>(
            false,
            Error: ex.Message
        );

        Console.WriteLine(
            JsonSerializer.Serialize(response, options)
        );
    }

    static BridgeResponse<ImageResponse> ReadFile(
        WpdDeviceReader reader,
        string deviceId,
        string path
     )
    {
        using var stream = reader.ReadFile(deviceId, path);

        using var memory = new MemoryStream();

        stream.CopyTo(memory);

        var base64 = Convert.ToBase64String(memory.ToArray());

        return new BridgeResponse<ImageResponse>(
            true,
            new ImageResponse("image/jpeg", base64)
        );
    }
    ;
}
