using TramPocketBackend.Extensions;
using TramPocketBackend.Models;
using TramPocketBackend.Models.Gtfs;
using TramPocketBackend.Utils;
using Route = TramPocketBackend.Models.Gtfs.Route;

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

    // stops getters

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

    // stoptimes

    public List<StopTime> GetStopTimesByTripId(string tripId)
    {
        StopTimesByTripId.TryGetValue(tripId, out var stopTimes);
        return stopTimes ?? [];
    }

    public List<StopTime> GetStopTimesByStopId(string stopId)
    {
        StopTimesByStopId.TryGetValue(stopId, out var stopTimes);
        return stopTimes ?? [];
    }

    public StopTime? GetStopTime(string stopId, string tripId)
    {
        var compositeKey = GtfsUtils.CreateStopTripKey(stopId, tripId);
        StopTimesByStopTripId.TryGetValue(compositeKey, out var stopTime);
        return stopTime;
    }

    // trips

    public Trip? GetTripById(string tripId)
    {
        TripsById.TryGetValue(tripId, out var trip);
        return trip;
    }

    // routes

    public Route? GetRouteById(string routeId)
    {
        RoutesById.TryGetValue(routeId, out var route);
        return route;
    }

    // services

    /// <summary>
    ///     Returns ids of services, which are running this day.
    /// </summary>
    public HashSet<string> GetServicesIdsForDate(DateTime date)
    {
        var serviceIds = new HashSet<string>();

        CalendarDatesByDate.TryGetValue(date.ConvertToGtfsCalendarDate(), out var calendarDates);
        if (calendarDates == null) return serviceIds;

        foreach (var calendarDate in calendarDates)
            switch (calendarDate.ExceptionType)
            {
                case 1:
                    serviceIds.Add(calendarDate.ServiceId);
                    break;
                case 2:
                    serviceIds.Remove(calendarDate.ServiceId);
                    break;
            }

        return serviceIds;
    }
}