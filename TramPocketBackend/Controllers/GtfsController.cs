using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TramPocketBackend.Models.Dtos;
using TramPocketBackend.Models.Dtos.Mappers;
using TramPocketBackend.State;

namespace TramPocketBackend.Controllers;

[Route("api/gtfs")]
[ApiController]
public class GtfsController(TransitState transitState) : ControllerBase
{
    [HttpGet("stop/id/{id}")]
    [ProducesResponseType(typeof(StopDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [OpenApiOperation("Get tram stop by ID", "Example: stop_252_42019")]
    public IActionResult GetStopById(string id)
    {
        var stop = transitState.GetStopById(id);
        if (stop == null) return NotFound();
        return Ok(stop.ToDto());
    }

    [HttpGet("stop/code/{code}")]
    [ProducesResponseType(typeof(StopDto), StatusCodes.Status200OK)]
    [OpenApiOperation("Get tram stop by code", "Example: 119-01")]
    public IActionResult GetStopByCode(string code)
    {
        var stop = transitState.GetStopByCode(code);
        if (stop == null) return NotFound();
        return Ok(stop.ToDto());
    }
}