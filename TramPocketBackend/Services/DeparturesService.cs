using Microsoft.Extensions.Options;
using TramPocketBackend.Configuration;
using TramPocketBackend.Models;
using TramPocketBackend.Models.Gtfs;
using TramPocketBackend.State;

namespace TramPocketBackend.Services;

public class DeparturesService(
    IOptions<DeparturesOptions> options,
    TransitState transitState)
{
    public List<Departure>? GetDeparturesFromStop(string stopCode, DateTime time)
    {
        var stop = transitState.GetStopByCode(stopCode);
        if (stop == null) return null;

        var gracePeriodCutoff = time.AddMinutes(-options.Value.GracePeriodMinutes);

        return GetGtfsDepartures(stop, gracePeriodCutoff);
    }

    private List<Departure> GetGtfsDepartures(Stop stop, DateTime time)
    {
        var departures = new List<Departure>();

        var services = transitState.GetServicesIdsForDate(time);
        var stopTimes = transitState.GetStopTimesByStopId(stop.StopId);

        var date = new DateOnly(time.Year, time.Month, time.Day);

        foreach (var stopTime in stopTimes)
        {
            var departureTime = date.ToDateTime(TimeOnly.MinValue).Add(stopTime.DepartureTime);
            if (departureTime < time) continue;

            var trip = transitState.GetTripById(stopTime.TripId);
            if (trip == null) continue;

            if (!services.Contains(trip.ServiceId)) continue;

            var route = transitState.GetRouteById(trip.RouteId);
            if (route == null) continue;

            departures.Add(new Departure
            {
                Line = route.RouteShortName,
                Destination = trip.TripHeadsign,
                DepartureTime = departureTime.ToString("HH:mm:ss")
            });
        }

        return departures;
    }
}