namespace TramPocketBackend.Models;

public record Departure
{
    public string Line { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string DepartureTime { get; set; } = string.Empty;
    public int? RtDelay { get; set; } = null;
    public int? RtDepartureTime { get; set; } = null;
}