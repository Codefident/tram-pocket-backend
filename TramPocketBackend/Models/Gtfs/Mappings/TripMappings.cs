using CsvHelper.Configuration;

namespace TramPocketBackend.Models.Gtfs.Mappings;

public sealed class TripMappings : ClassMap<Trip>
{
    public TripMappings()
    {
        Map(m => m.TripId).Name("trip_id");
        Map(m => m.RouteId).Name("route_id");
        Map(m => m.ServiceId).Name("service_id");
        Map(m => m.TripHeadsign).Name("trip_headsign");
        Map(m => m.TripShortName).Name("trip_short_name");
        Map(m => m.DirectionId).Name("direction_id");
        Map(m => m.BlockId).Name("block_id");
        Map(m => m.ShapeId).Name("shape_id");
        Map(m => m.WheelchairAccessible).Name("wheelchair_accessible");
    }
}