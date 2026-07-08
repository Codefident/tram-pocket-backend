namespace TramPocketBackend.Models.Gtfs;

public abstract record StopTime
{
    public string TripId { get; init; } = string.Empty;
    public string ArrivalTime { get; init; } = string.Empty;
    public string DepartureTime { get; init; } = string.Empty;
    public string StopId { get; init; } = string.Empty;
    public int StopSequence { get; init; }
    public string StopHeadsign { get; init; } = string.Empty;
    public int PickupType { get; init; } = 0;
    public int DropOffType { get; init; } = 0;
    public float ShapeDistTraveled { get; init; } = 0.0f;
    public int Timepoint { get; init; } = 0;
}