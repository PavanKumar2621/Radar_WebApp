namespace WebSocketDemo.Models;

public class CommandResponseData
{
    public bool Success { get; set; }

    public string Command { get; set; } = "";

    public string Message { get; set; } = "";
}