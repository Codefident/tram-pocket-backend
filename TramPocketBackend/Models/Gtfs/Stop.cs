namespace TramPocketBackend.Models.Gtfs;

public record Stop
{
    public string StopId { get; init; } = string.Empty;
    public string StopCode { get; init; } = string.Empty;
    public string StopName { get; init; } = string.Empty;
    public string StopDesc { get; init; } = string.Empty;
    public double StopLat { get; init; }
    public double StopLon { get; init; }
    public string ZoneId { get; init; } = string.Empty;
    public string StopUrl { get; init; } = string.Empty;
    public int LocationType { get; init; }
    public string ParentStation { get; init; } = string.Empty;
    public string StopTimezone { get; init; } = string.Empty;
    public string WheelchairBoarding { get; init; } = string.Empty;
    public string PlatformCode { get; init; } = string.Empty;
}