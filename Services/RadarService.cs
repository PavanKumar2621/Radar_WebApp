using WebSocketDemo.Models;

namespace WebSocketDemo.Services;

public class RadarService
{
    private readonly object _lock = new();

    // Store latest data for each Track ID
    private readonly Dictionary<int, RadarData> _tracks = new();

    public void UpdateRadarData(RadarData data)
    {
        lock (_lock)
        {
            _tracks[data.TrackId] = data;
        }
    }

    public RadarData? GetTrackData(int trackId)
    {
        lock (_lock)
        {
            if (_tracks.TryGetValue(trackId, out var data))
            {
                return data;
            }
            return null;
        }
    }

    public List<RadarData> GetAllTracks()
    {
        lock (_lock)
        {
            return _tracks.Values.ToList();
        }
    }
}