using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TramPocketBackend.Models.Dtos;
using TramPocketBackend.Models.Dtos.Mappers;
using TramPocketBackend.Services;
using TramPocketBackend.State;

namespace TramPocketBackend.Controllers;

[Route("api/stops")]
[ApiController]
public class StopsController(DeparturesService departuresService, TransitState transitState) : ControllerBase
{
    [HttpGet("{code}/details")]
    [ProducesResponseType(typeof(StopDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [OpenApiOperation("Get tram stop details by code", "Example: 119-01")]
    public IActionResult GetStopByCode(string code)
    {
        var stop = transitState.GetStopByCode(code);
        if (stop == null) return NotFound();
        return Ok(stop.ToDto());
    }

    [HttpGet("{code}/departures")]
    [ProducesResponseType(typeof(DeparturesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [OpenApiOperation("Get departures from stop", "Example: 119-01")]
    public IActionResult GetDeparturesFromStop(string code)
    {
        var departures = departuresService.GetDeparturesFromStop(code, DateTime.Now);
        if (departures == null) return NotFound("Stop not found");

        var response = new DeparturesDto
        {
            StopName = transitState.GetStopByCode(code)!.StopName,
            Departures = departures
        };

        return Ok(response);
    }
}