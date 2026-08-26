using System.Text.Json;
using WebSocketDemo.Models;
using WebSocketDemo.Services;

namespace WebSocketDemo.Handlers;

public class CommandHandler
{
    private readonly ServoService _servoService;

    public CommandHandler(ServoService servoService)
    {
        _servoService = servoService;
    }

    public async Task<Message<CommandResponseData>> Handle(
        Message<object> message)
    {
        Console.WriteLine("=================================");
        Console.WriteLine("CommandHandler");
        Console.WriteLine("=================================");

        Console.WriteLine($"Message Type : {message.MessageType}");
        Console.WriteLine($"Request ID   : {message.RequestId}");
        Console.WriteLine($"Source       : {message.Source}");
        Console.WriteLine($"Timestamp    : {message.Timestamp}");

        var json = JsonSerializer.Serialize(message.Data);

        Console.WriteLine($"Raw Command Data : {json}");

        var commandData =
            JsonSerializer.Deserialize<CommandData>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

        if (commandData == null)
            return CreateResponse(message, false, "", "Invalid command data");

        Console.WriteLine($"Command      : {commandData.Command}");

        bool success;
        string responseMessage;

        switch (commandData.Command)
        {
            case "startScan":
                success = _servoService.StartScan();
                responseMessage = success ? "Scan started" : "Failed to start scan";
                break;

            case "stopScan":
                success = _servoService.StopScan();
                responseMessage = success ? "Scan stopped" : "Failed to stop scan";
                break;

            case "setAzimuth":
                if (commandData.Parameters == null)
                    return CreateResponse(message, false, "setAzimuth", "Missing parameters");

                var parameterJson = JsonSerializer.Serialize(commandData.Parameters);
                var azimuthData =
                    JsonSerializer.Deserialize<SetAzimuthData>(
                        parameterJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                if (azimuthData == null)
                    return CreateResponse(message, false, "setAzimuth", "Invalid azimuth parameters");

                Console.WriteLine($"Azimuth      : {azimuthData.Azimuth}");
                success = _servoService.SetAzimuth(azimuthData.Azimuth);
                responseMessage = success ? "Azimuth set successfully" : "Failed to set azimuth";
                break;

            default:
                Console.WriteLine($"Unknown command: {commandData.Command}");
                success = false;
                responseMessage = $"Unknown command: {commandData.Command}";
                break;
        }

        return CreateResponse(message, success, commandData.Command, responseMessage);
    }


    private Message<CommandResponseData> CreateResponse(Message<object> request, bool success, string command, string responseMessage)
    {
        return new Message<CommandResponseData>
        {
            MessageType = "commandResponse",
            RequestId = request.RequestId,
            Timestamp = DateTime.UtcNow,
            Source = "server",
            Data = new CommandResponseData
            {
                Success = success,
                Command = command,
                Message = responseMessage
            }
        };
    }
}