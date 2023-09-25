using System;
using System.IO;
using NAudio.CoreAudioApi;

class Program
{
    static void Main(string[] args)
    {
        string targetDeviceName;
        bool isReadFromFile = false;
        string deviceNameFilePath = "DeviceName.txt";

        MMDeviceCollection inputDevices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);


        // Check if command-line arguments are provided
        if (args.Length > 0)
        {
            targetDeviceName = ReadDeviceFromArgument(args);
        }
        // Check Device Name from file or user input
        else
        {
            if (File.Exists(deviceNameFilePath))
            {
                isReadFromFile = true;
                targetDeviceName = ReadDeviceFromFile(deviceNameFilePath);
            }
            else
            {
                // Read the device name from user input
                targetDeviceName = ChooseDeviceFromList(inputDevices);
            }
        }

        // Find the index of the target device by name
        int deviceIndex = FindIndexDeviceByName(targetDeviceName, inputDevices);

        if (deviceIndex == -1 && isReadFromFile)
        {
            targetDeviceName = ChooseDeviceFromList(inputDevices);
            deviceIndex = FindIndexDeviceByName(targetDeviceName, inputDevices);

            if (deviceIndex == -1)
            {
                return;
            }
        }
        else if (deviceIndex == -1)
        {
            Console.WriteLine($"Device '{targetDeviceName}' not found among enabled devices.");
            return;
        }

        MMDevice targetDevice = inputDevices[deviceIndex];

        // Toggle the mute Settings
        bool isDeviceMuted = !targetDevice.AudioEndpointVolume.Mute;
        targetDevice.AudioEndpointVolume.Mute = isDeviceMuted;

        string deviceState;
        if (isDeviceMuted)
        {
            deviceState = "Disable";
        }
        else
        {
            deviceState = "Enabled";
        }
        Console.WriteLine($"'{targetDeviceName}' changed to {deviceState}.");
        Console.WriteLine($"Press any Key to exit...");
        try
        {
            Console.ReadKey();

        }
        catch (System.InvalidOperationException)
        {
            Console.ReadLine();
        }
        return;
    }

    private static int FindIndexDeviceByName(string targetDeviceName, MMDeviceCollection devices)
    {
        for (int i = 0; i < devices.Count; i++)
        {
            if (devices[i].FriendlyName == targetDeviceName)
            {
                return i;
            }
        }
        Console.WriteLine($"Device '{targetDeviceName}' not found among enabled devices.");
        return -1;
    }

    private static string ReadDeviceFromArgument(string[] args)
    {
        return args[0];
    }

    private static string ReadDeviceFromFile(string deviceNameFilePath)
    {

        return File.ReadAllText(deviceNameFilePath).Trim();
    }

    private static string ChooseDeviceFromList(MMDeviceCollection inputDevices)
    {
        Console.WriteLine("List of Enabled Input Audio Devices:");
        for (int i = 0; i < inputDevices.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {inputDevices[i].FriendlyName}");
        }

        Console.Write("Enter the number of the device you want to toggle 'Listen to this device' (or 0 to exit): ");
        if (int.TryParse(Console.ReadLine(), out int deviceIndex))
        {
            if (deviceIndex == 0)
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            }
            else if (deviceIndex >= 1 && deviceIndex <= inputDevices.Count)
            {
                return inputDevices[deviceIndex - 1].FriendlyName;
            }
        }

        Console.WriteLine("Invalid device number.");
        Environment.Exit(1);
        return null;
    }
}
