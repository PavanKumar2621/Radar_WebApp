namespace WebSocketDemo.Models;

public class Message<T>
{
    public string MessageType { get; set; } = "";

    public string? RequestId { get; set; }

    public DateTime Timestamp { get; set; }

    public string Source { get; set; } = "";

    public T? Data { get; set; }
}