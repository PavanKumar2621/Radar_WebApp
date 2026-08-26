using WebSocketDemo.Handlers;
using WebSocketDemo.Models;
namespace WebSocketDemo.Services;

public class MessageRouter
{
    private readonly CommandHandler _commandHandler;

    public MessageRouter(CommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task<Message<CommandResponseData>?> ProcessMessage(Message<object> message)
    {
        Console.WriteLine($"Routing message: {message.MessageType}");

        switch (message.MessageType)
        {
            case "command":
                return await _commandHandler.Handle(message);

            case "radarData":
                Console.WriteLine("Radar data received");
                return null;

            case "configuration":
                Console.WriteLine("Configuration message received");
                return null;

            default:
                Console.WriteLine($"Unknown message type: {message.MessageType}");
                return null;
        }
    }
}