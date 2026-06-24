using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TramPocketBackend.Models.Dtos;
using TramPocketBackend.State;

namespace TramPocketBackend.Controllers;

[Route("api/gtfs")]
[ApiController]
public class GtfsController(TransitState transitState) : ControllerBase
{
    [HttpGet("stop/{id}")]
    [ProducesResponseType(typeof(StopDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [OpenApiOperation("Get tram stop by ID", "")]
    public IActionResult GetStop(string id)
    {
        if (!transitState.StopsById.TryGetValue(id, out var stop)) return NotFound();

        var stopDto = new StopDto
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

        return Ok(stopDto);
    }
}