using TramPocketBackend.Models.Gtfs;

namespace TramPocketBackend.Models.Dtos;

public static class StopDtoMapper
{
    public static StopDto ToDto(this Stop stop)
    {
        return new StopDto
        {
            StopId = stop.StopId,
            StopCode = stop.StopCode,
            StopName = stop.StopName,
            StopDesc = stop.StopDesc,
            StopLat = stop.StopLat,
            StopLon = stop.StopLon,
            ZoneId = stop.ZoneId,
            StopUrl = stop.StopUrl,
            LocationType = stop.LocationType,
            ParentStation = stop.ParentStation,
            StopTimezone = stop.StopTimezone,
            WheelchairBoarding = stop.WheelchairBoarding,
            PlatformCode = stop.PlatformCode
        };
    }
}