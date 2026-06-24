using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using TramPocketBackend.Configuration;
using TramPocketBackend.Mappings.Gtfs;
using TramPocketBackend.Models;
using TramPocketBackend.Models.Gtfs;
using Route = TramPocketBackend.Models.Gtfs.Route;

namespace TramPocketBackend.Services.Gtfs;

public class GtfsParserService(ILogger<GtfsParserService> logger, IOptions<GtfsOptions> options, IHostEnvironment env)
{
    private readonly string _gtfsExtractPath = Path.Combine(env.ContentRootPath, options.Value.ExtractPath);

    public TransitStateData Parse()
    {
        var stops = ParseStops();
        var routes = ParseRoutes();
        var trips = ParseTrips();
        var calendarDates = ParseCalendarDates();

        return new TransitStateData
        {
            StopsById = stops.StopsById,
            StopsByCode = stops.StopsByCode,
            RoutesById = routes,
            TripsById = trips,
            CalendarDatesByServiceId = calendarDates.CalendarDatesByServiceId,
            CalendarDatesByDate = calendarDates.CalendarDatesByDate
        };
    }

    private (
        Dictionary<string, Stop> StopsById,
        Dictionary<string, Stop> StopsByCode
        ) ParseStops()
    {
        var stopsById = new Dictionary<string, Stop>();
        var stopsByCode = new Dictionary<string, Stop>();

        var stops = GetRecordsFromFile<StopMappings, Stop>("stops.txt");
        foreach (var stop in stops)
        {
            stopsById[stop.StopId] = stop;
            if (!string.IsNullOrWhiteSpace(stop.StopCode))
                stopsByCode[stop.StopCode] = stop;
        }

        logger.LogInformation("Parsed {n} stops", stopsById.Count);
        return (stopsById, stopsByCode);
    }

    private Dictionary<string, Route> ParseRoutes()
    {
        var routesById = new Dictionary<string, Route>();

        var routes = GetRecordsFromFile<RouteMappings, Route>("routes.txt");
        foreach (var route in routes) routesById[route.RouteId] = route;

        logger.LogInformation("Parsed {n} routes", routesById.Count);
        return routesById;
    }

    private Dictionary<string, Trip> ParseTrips()
    {
        var tripsById = new Dictionary<string, Trip>();

        var trips = GetRecordsFromFile<TripMappings, Trip>("trips.txt");
        foreach (var trip in trips) tripsById[trip.TripId] = trip;

        logger.LogInformation("Parsed {n} trips", tripsById.Count);
        return tripsById;
    }

    private (
        Dictionary<string, List<CalendarDate>> CalendarDatesByServiceId,
        Dictionary<string, List<CalendarDate>> CalendarDatesByDate
        ) ParseCalendarDates()
    {
        var calendarDatesByServiceId = new Dictionary<string, List<CalendarDate>>();
        var calendarDatesByDate = new Dictionary<string, List<CalendarDate>>();

        var calendarDates = GetRecordsFromFile<CalendarDateMappings, CalendarDate>("calendar_dates.txt");
        foreach (var calendarDate in calendarDates)
        {
            if (!calendarDatesByServiceId.ContainsKey(calendarDate.ServiceId))
                calendarDatesByServiceId[calendarDate.ServiceId] = new List<CalendarDate>();
            calendarDatesByServiceId[calendarDate.ServiceId].Add(calendarDate);

            if (!calendarDatesByDate.ContainsKey(calendarDate.Date))
                calendarDatesByDate[calendarDate.Date] = new List<CalendarDate>();
            calendarDatesByDate[calendarDate.Date].Add(calendarDate);
        }

        logger.LogInformation("Parsed {n} calendar dates", calendarDates.Count);
        return (calendarDatesByServiceId, calendarDatesByDate);
    }

    private List<TModel> GetRecordsFromFile<TMap, TModel>(string fileName) where TMap : ClassMap
    {
        using var reader = new StreamReader(Path.Combine(_gtfsExtractPath, fileName));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TMap>();

        return csv.GetRecords<TModel>().ToList();
    }
}