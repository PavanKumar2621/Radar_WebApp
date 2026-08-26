using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR;
using WebSocketDemo.Hubs;
using WebSocketDemo.Models;
using WebSocketDemo.Services;
namespace WebSocketDemo.Services;

public class RadarUdpReceiver : BackgroundService
{
    private const int PORT = 5005;

    private readonly RadarService _radarService;
    private readonly IHubContext<CommunicationHub> _hubContext;

    public RadarUdpReceiver(RadarService radarService, IHubContext<CommunicationHub> hubContext)
    {
        _radarService = radarService;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using UdpClient udpClient = new UdpClient(PORT);

        Console.WriteLine(
            $"Radar UDP receiver started on port {PORT}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                UdpReceiveResult result =
                    await udpClient.ReceiveAsync(stoppingToken);

                byte[] packet = result.Buffer;

                Console.WriteLine(
                    $"UDP packet received: {packet.Length} bytes");

                ParsePacket(packet);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine(
                "Radar UDP receiver stopped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Radar UDP receiver error: {ex.Message}");
        }
    }

    private async void ParsePacket(byte[] packet)
    {
        // Expected packet size
        if (packet.Length < 23)
        {
            Console.WriteLine($"Invalid packet size: {packet.Length}");
            return;
        }
        // Console.WriteLine(BitConverter.ToString(packet));

        byte trackId = packet[2]; 
        
        float azimuth = BitConverter.ToSingle(packet, 3);   // Bytes 3,4,5,6
        float elevation = BitConverter.ToSingle(packet, 7);   // Bytes 7,8,9,10
        float range = BitConverter.ToSingle(packet, 11);  // Bytes 11,12,13,14
        float velocity = BitConverter.ToSingle(packet, 15);  // Bytes 15,16,17,18
        float snr = BitConverter.ToSingle(packet, 19);  // Bytes 19,20,21,22

        var radarData = new RadarData
        {
            TrackId = trackId,
            Azimuth = azimuth,
            Elevation = elevation,
            Range = range,
            Velocity = velocity,
            Snr = snr
        };

        _radarService.UpdateRadarData(radarData);

        var message = new Message<RadarData>
        {
            MessageType = "radarData",
            RequestId = $"RADAR-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            Timestamp = DateTime.UtcNow,
            Source = "radar",
            Data = radarData
        };

        await _hubContext.Clients.All.SendAsync("ReceiveRadarData", message);
    }
}