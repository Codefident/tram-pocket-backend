using Microsoft.Extensions.Options;
using TramPocketBackend.Configuration;
using TramPocketBackend.State;
using TransitRealtime;

namespace TramPocketBackend.Services;

public class RealtimeService(
    ILogger<RealtimeService> logger,
    IOptions<RealtimeOptions> options,
    HttpClient httpClient,
    TransitState transitState)
    : BackgroundService
{
    private readonly RealtimeOptions _options = options.Value;

    private async Task FetchTripUpdatesAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching Realtime TripUpdates from {url}", _options.TripUpdatesTUrl);

        try
        {
            using var response = await httpClient.GetAsync(
                _options.TripUpdatesTUrl,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            await using var httpStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var feedMessage = FeedMessage.Parser.ParseFrom(httpStream);

            transitState.UpdateTripUpdates(feedMessage.Entity
                .Where(e => e.TripUpdate != null)
                .ToDictionary(e => e.Id, e => e.TripUpdate));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured while fetching Realtime TripUpdates");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await FetchTripUpdatesAsync(cancellationToken);
            await Task.Delay(_options.TripUpdatesIntervalSeconds * 1000, cancellationToken);
        }
    }
}