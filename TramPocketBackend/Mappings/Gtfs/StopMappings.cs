using CsvHelper.Configuration;
using TramPocketBackend.Models.Gtfs;

namespace TramPocketBackend.Mappings.Gtfs;

public sealed class StopMappings : ClassMap<StopModel>
{
    public StopMappings()
    {
        Map(m => m.StopId).Name("stop_id");
        Map(m => m.StopCode).Name("stop_code");
        Map(m => m.StopName).Name("stop_name");
        Map(m => m.StopDesc).Name("stop_desc");
        Map(m => m.StopLat).Name("stop_lat");
        Map(m => m.StopLon).Name("stop_lon");
        Map(m => m.ZoneId).Name("zone_id");
        Map(m => m.StopUrl).Name("stop_url");
        Map(m => m.LocationType).Name("location_type");
        Map(m => m.ParentStation).Name("parent_station");
        Map(m => m.StopTimezone).Name("stop_timezone");
        Map(m => m.WheelchairBoarding).Name("wheelchair_boarding");
        Map(m => m.PlatformCode).Name("platform_code");
    }
}