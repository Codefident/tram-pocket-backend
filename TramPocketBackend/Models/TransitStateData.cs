using TramPocketBackend.Models.Gtfs;
using Route = TramPocketBackend.Models.Gtfs.Route;

namespace TramPocketBackend.Models;

public class TransitStateData
{
    // stops
    public Dictionary<string, Stop> StopsById { get; set; } = new();
    public Dictionary<string, Stop> StopsByCode { get; set; } = new();

    // routes
    public Dictionary<string, Route> RoutesById { get; set; } = new();

    // trips
    public Dictionary<string, Trip> TripsById { get; set; } = new();

    // calendar dates
    public Dictionary<string, List<CalendarDate>> CalendarDatesByServiceId { get; set; } = new();
    public Dictionary<string, List<CalendarDate>> CalendarDatesByDate { get; set; } = new();
}