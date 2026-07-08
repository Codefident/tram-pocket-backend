using Microsoft.Extensions.Options;
using TramPocketBackend.Configuration;
using TramPocketBackend.Models.Gtfs;
using TramPocketBackend.State;

namespace TramPocketBackend.Services;

public class DeparturesService(
    ILogger<DeparturesService> logger,
    IOptions<DeparturesOptions> options,
    TransitState transitState)
{
    public void GetDepartures(string stopId, DateTime fromTime)
    {
        var stop = transitState.GetStopById(stopId);
        if (stop == null) return; // TODO: add "null"
    }

    private void GetGtfsDepartures(Stop stop, DateTime fromTime)
    {
        var gracePeriodCutoff = fromTime.AddMinutes(-options.Value.GracePeriodMinutes);
    }
}