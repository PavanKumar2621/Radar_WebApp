namespace WebSocketDemo.Models;

public class DeviceStatus
{
    public string DeviceId { get; set; } = "";

    public bool Connected { get; set; }

    public string Status { get; set; } = "";

    public double Temperature { get; set; }
}