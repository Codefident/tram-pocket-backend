namespace TramPocketBackend.Models.Gtfs;

public record TripModel
{
    public string TripId { get; set; } = string.Empty;
    public string RouteId { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string TripHeadsign { get; set; } = string.Empty;
    public string TripShortName { get; set; } = string.Empty;
    public int DirectionId { get; set; }
    public string BlockId { get; set; } = string.Empty;
    public string ShapeId { get; set; } = string.Empty;
    public int WheelchairAccessible { get; set; }
}