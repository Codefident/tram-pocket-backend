namespace TramPocketBackend.Configuration;

public class RealtimeOptions
{
    public string DataPath { get; set; } = string.Empty;
    public string TripUpdatesTUrl { get; set; } = string.Empty;
    public string TripUpdatesPath { get; set; } = string.Empty;
    public int TripUpdatesIntervalSeconds { get; set; } = 30;
}