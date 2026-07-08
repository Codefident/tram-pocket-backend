using TramPocketBackend.Models;
using TramPocketBackend.Models.Gtfs;
using TramPocketBackend.Utils;

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
        StopTimesByTripId = data.StopTimesByTripId;
        StopTimesByStopId = data.StopTimesByStopId;
        StopTimesByStopTripId = data.StopTimesByStopTripId;
    }

    public Stop? GetStopById(string id)
    {
        StopsById.TryGetValue(id, out var stop);
        return stop;
    }

    public Stop? GetStopByCode(string code)
    {
        StopsByCode.TryGetValue(code, out var stop);
        return stop;
    }

    public List<StopTime> GetStopTimesByTripId(string tripId)
    {
        StopTimesByTripId.TryGetValue(tripId, out var stopTimes);
        return stopTimes ?? new List<StopTime>();
    }

    public List<StopTime> GetStopTimesByStopId(string stopId)
    {
        StopTimesByStopId.TryGetValue(stopId, out var stopTimes);
        return stopTimes ?? new List<StopTime>();
    }

    public StopTime? GetStopTime(string stopId, string tripId)
    {
        var compositeKey = GtfsUtils.CreateStopTripKey(stopId, tripId);
        StopTimesByStopTripId.TryGetValue(compositeKey, out var stopTime);
        return stopTime;
    }
}