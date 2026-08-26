namespace WebSocketDemo.Services;

public class ServoService
{
    public bool StartScan()
    {
        Console.WriteLine("ServoService: Starting scan...");
        return true;
    }

    public bool StopScan()
    {
        Console.WriteLine("ServoService: Stopping scan...");
        return true;
    }

    public bool SetAzimuth(double azimuth)
    {
        Console.WriteLine($"ServoService: Setting azimuth to {azimuth}°");
        return true;
    }
}