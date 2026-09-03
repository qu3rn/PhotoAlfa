import { spawn } from "child_process";
import readline from "node:readline";

const bridge = spawn("dotnet", [
    "windows-device-bridge\\obj\\Debug\\net10.0-windows7.0\\windows-device-bridge.dll"
]);

const output = readline.createInterface({
    input: bridge.stdout
});

output.on('line', (line) =>
{
    console.log("bridge response:", line);
});

bridge.stderr.on('data', (data) =>
{
    console.error('bridge error:', data.toString());
});

export function getDevices()
{
    bridge.stdin.write(
        JSON.stringify({
            command: 'devices'
        }) + '\n'
    );
}
