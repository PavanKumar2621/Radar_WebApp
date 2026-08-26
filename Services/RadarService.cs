using WebSocketDemo.Models;

namespace WebSocketDemo.Services;

// public class RadarService
// {
//     private readonly Random _random = new();

//     public RadarData GetRadarData()
//     {
//         return new RadarData
//         {
//             TrackId = 1,
//             Azimuth = Math.Round(_random.NextDouble() * 360.0, 2),
//             Elevation = Math.Round(_random.NextDouble() * 90.0, 2),
//             Range = Math.Round(500 + _random.NextDouble() * 4500, 2),
//             Velocity = Math.Round(-50 + _random.NextDouble() * 100, 2),
//             Snr = Math.Round(10 + _random.NextDouble() * 30, 2)
//         };
//     }
// }

// public class RadarService
// {
//     private readonly Random _random = new();
//     private static double time = 0;

    // public RadarData GetRadarData()
    // {
    //     time += 0.1; // Increment time for animation effect
    //     return new RadarData
    //     {
    //         TrackId = 1,
    //         Azimuth = 180 + 30 * Math.Sin(time),
    //         Elevation = 45 + 10 * Math.Sin(time * 0.5),
    //         Range = 2000 + 500 * Math.Sin(time * 0.2),
    //         Velocity = 50 + 10 * Math.Sin(time),
    //         Snr = 30 + 5 * Math.Sin(time * 0.7)
    //     };
    // }
// }


public class RadarService
{
    private readonly object _lock = new();

    private RadarData _latestData = new()
    {
        TrackId = 0,
        Azimuth = 0,
        Elevation = 0,
        Range = 0,
        Velocity = 0,
        Snr = 0
    };

    public void UpdateRadarData(RadarData data)
    {
        lock (_lock)
        {
            _latestData = data;
        }
    }

    public RadarData GetRadarData()
    {
        lock (_lock)
        {
            return _latestData;
        }
    }
}
