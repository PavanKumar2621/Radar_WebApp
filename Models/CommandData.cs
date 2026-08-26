namespace WebSocketDemo.Models;

public class CommandData
{
    public string Command { get; set; } = "";

    public Dictionary<string, object>? Parameters { get; set; }
}