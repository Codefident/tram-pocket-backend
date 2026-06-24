using TramPocketBackend.Models.Gtfs;

namespace TramPocketBackend.Models;

public class TransitStateDataModel
{
    // stops
    public Dictionary<string, StopModel> StopsById { get; set; } = new();
    public Dictionary<string, StopModel> StopsByCode { get; set; } = new();

    // routes
    public Dictionary<string, RouteModel> RoutesById { get; set; } = new();

    // trips
    public Dictionary<string, TripModel> TripsById { get; set; } = new();

    // calendar dates
    public Dictionary<string, List<CalendarDateModel>> CalendarDatesByServiceId { get; set; } = new();
    public Dictionary<string, List<CalendarDateModel>> CalendarDatesByDate { get; set; } = new();
}