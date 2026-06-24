using TramPocketBackend.Models;

namespace TramPocketBackend.State;

public sealed class TransitState : TransitStateDataModel
{
    public void UpdateData(TransitStateDataModel dataModel)
    {
        StopsById = dataModel.StopsById;
        StopsByCode = dataModel.StopsByCode;
        RoutesById = dataModel.RoutesById;
        TripsById = dataModel.TripsById;
        CalendarDatesByServiceId = dataModel.CalendarDatesByServiceId;
        CalendarDatesByDate = dataModel.CalendarDatesByDate;
    }
}