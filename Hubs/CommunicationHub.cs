using Microsoft.AspNetCore.SignalR;
using WebSocketDemo.Models;
using WebSocketDemo.Services;

namespace WebSocketDemo.Hubs;

public class CommunicationHub : Hub
{
    private readonly MessageRouter _messageRouter;
    private readonly RadarService _radarService;

    public CommunicationHub(MessageRouter messageRouter, RadarService radarService)
    {
        _messageRouter = messageRouter;
        _radarService = radarService;
    }

    public async Task SendMessage(Message<object> message)
    {
        Console.WriteLine($"Message received: {message.MessageType}");
        var response = await _messageRouter.ProcessMessage(message);
        if (response != null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", response);
        }
    }

    public async Task SendRadarData()
    {
        var radarData = _radarService.GetRadarData();

        var message = new Message<RadarData>
        {
            MessageType = "radarData",
            RequestId = $"RADAR-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            Timestamp = DateTime.UtcNow,
            Source = "radar",
            Data = radarData
        };

        await Clients.Caller.SendAsync("ReceiveRadarData", message);
    }
}