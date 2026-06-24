using TramPocketBackend.Models;

namespace TramPocketBackend.State;

public sealed class TransitState : TransitStateData
{
    public void UpdateData(TransitStateData data)
    {
        StopsById = data.StopsById;
        StopsByCode = data.StopsByCode;
        RoutesById = data.RoutesById;
        TripsById = data.TripsById;
        CalendarDatesByServiceId = data.CalendarDatesByServiceId;
        CalendarDatesByDate = data.CalendarDatesByDate;
    }
}