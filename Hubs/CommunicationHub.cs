using Microsoft.AspNetCore.SignalR;
using WebSocketDemo.Models;
using WebSocketDemo.Services;

namespace WebSocketDemo.Hubs;

public class CommunicationHub : Hub
{
    private readonly MessageRouter _messageRouter;

    public CommunicationHub(MessageRouter messageRouter)
    {
        _messageRouter = messageRouter;
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
}