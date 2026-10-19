using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TramPocketBackend.Models.Gtfs.Mappings;

public sealed class GtfsTimeConverter : DefaultTypeConverter
{
    /// <summary>
    ///     Parses GTFS time format (HH:MM:SS) to TimeSpan.
    ///     GTFS times can exceed 24 hours (e.g., 25:30:00 for overnight services).
    /// </summary>
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return TimeSpan.Zero;

        var parts = text.Split(':');
        if (parts.Length != 3 ||
            !int.TryParse(parts[0], out var hours) ||
            !int.TryParse(parts[1], out var minutes) ||
            !int.TryParse(parts[2], out var seconds)
           )
            return TimeSpan.Zero;

        return new TimeSpan(hours, minutes, seconds);
    }
}

public sealed class StopTimeMap : ClassMap<StopTime>
{
    public StopTimeMap()
    {
        Map(m => m.TripId).Name("trip_id");
        Map(m => m.ArrivalTime).Name("arrival_time").TypeConverter<GtfsTimeConverter>();
        Map(m => m.DepartureTime).Name("departure_time").TypeConverter<GtfsTimeConverter>();
        Map(m => m.StopId).Name("stop_id");
        Map(m => m.StopSequence).Name("stop_sequence");
        Map(m => m.StopHeadsign).Name("stop_headsign");
        Map(m => m.PickupType).Name("pickup_type");
        Map(m => m.DropOffType).Name("drop_off_type");
        Map(m => m.ShapeDistTraveled).Name("shape_dist_traveled");
        Map(m => m.Timepoint).Name("timepoint");
    }
}