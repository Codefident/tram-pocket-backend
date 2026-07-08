using CsvHelper.Configuration;

namespace TramPocketBackend.Models.Gtfs.Mappings;

public sealed class StopTimeMap : ClassMap<StopTime>
{
    public StopTimeMap()
    {
        Map(m => m.TripId).Name("trip_id");
        Map(m => m.ArrivalTime).Name("arrival_time");
        Map(m => m.DepartureTime).Name("departure_time");
        Map(m => m.StopId).Name("stop_id");
        Map(m => m.StopSequence).Name("stop_sequence");
        Map(m => m.StopHeadsign).Name("stop_headsign");
        Map(m => m.PickupType).Name("pickup_type");
        Map(m => m.DropOffType).Name("drop_off_type");
        Map(m => m.ShapeDistTraveled).Name("shape_dist_traveled");
        Map(m => m.Timepoint).Name("timepoint");
    }
}