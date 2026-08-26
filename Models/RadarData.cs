namespace WebSocketDemo.Models;

public class RadarData
{
    public int TrackId { get; set; }

    public double Azimuth { get; set; }

    public double Elevation { get; set; }

    public double Range { get; set; }

    public double Velocity { get; set; }

    public double Snr { get; set; }
}