using System.IO.Compression;
using Microsoft.Extensions.Options;
using TramPocketBackend.Configuration;
using TramPocketBackend.Extensions;
using TramPocketBackend.State;

namespace TramPocketBackend.Services.Gtfs;

public class GtfsService(
    ILogger<GtfsService> logger,
    IOptions<GtfsOptions> options,
    HttpClient httpClient,
    GtfsParserService gtfsParserService,
    TransitState transitState,
    IHostEnvironment env)
    : BackgroundService
{
    private readonly string _gtfsExtractPath = Path.Combine(env.ContentRootPath, options.Value.ExtractPath);
    private readonly string _gtfsZipPath = Path.Combine(env.ContentRootPath, options.Value.ZipPath);
    private readonly GtfsOptions _options = options.Value;

    private bool GtfsFileExists()
    {
        return File.Exists(_gtfsZipPath);
    }

    private bool GtfsExtractedFilesExist()
    {
        return File.Exists(Path.Combine(_gtfsExtractPath, "stops.txt")) &&
               File.Exists(Path.Combine(_gtfsExtractPath, "routes.txt")) &&
               File.Exists(Path.Combine(_gtfsExtractPath, "trips.txt")) &&
               File.Exists(Path.Combine(_gtfsExtractPath, "stop_times.txt"));
    }

    private bool GtfsFileIsFresh()
    {
        if (!GtfsFileExists()) return false;

        // TODO: Consider switching to using UTC
        var lastWriteTime = File.GetLastWriteTime(_gtfsZipPath);
        var lastScheduledRefreshTime = new DateTime(
            DateTime.Now.Year,
            DateTime.Now.Month,
            DateTime.Now.Day,
            _options.DailyFetchHour,
            _options.DailyFetchMinute,
            _options.DailyFetchSecond
        );

        // GTFS files are fetched in the early morning, so
        // the one fetched day before is still fresh during the night.
        if (DateTime.Now.Hour < _options.DailyFetchHour)
            lastScheduledRefreshTime = lastScheduledRefreshTime.AddDays(-1);

        return lastWriteTime >= lastScheduledRefreshTime;
    }

    private bool GtfsExtractedFilesAreFresh()
    {
        if (!GtfsExtractedFilesExist()) return false;

        // TODO: Consider switching to using UTC
        var lastWriteTime = Directory.GetLastWriteTime(_gtfsExtractPath);
        var lastScheduledRefreshTime = new DateTime(
            DateTime.Now.Year,
            DateTime.Now.Month,
            DateTime.Now.Day,
            _options.DailyFetchHour,
            _options.DailyFetchMinute,
            _options.DailyFetchSecond
        );

        // GTFS files are fetched in the early morning, so
        // the one fetched day before is still fresh during the night.
        if (DateTime.Now.Hour < _options.DailyFetchHour)
            lastScheduledRefreshTime = lastScheduledRefreshTime.AddDays(-1);

        return lastWriteTime >= lastScheduledRefreshTime;
    }

    private async Task FetchGtfsAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching GTFS from {url}...", _options.KrkTUrl);

        try
        {
            using var response = await httpClient.GetAsync(
                _options.KrkTUrl,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            await using var httpStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            await using var fileStream =
                new FileStream(_gtfsZipPath, FileMode.Create, FileAccess.Write, FileShare.None);

            await httpStream.CopyToAsync(fileStream, cancellationToken);

            logger.LogInformation("GTFS zip file saved to {path}", _gtfsZipPath);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured while fetching GTFS");
        }
    }

    private async Task ExtractGtfsAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Extracting GTFS zip to {dir}", _gtfsExtractPath);

        try
        {
            if (!GtfsFileExists())
            {
                logger.LogWarning("Cannot extract GTFS zip, because the file does not exits");
                return;
            }

            // remove old files
            Directory.Delete(_gtfsExtractPath, true);
            Directory.CreateDirectory(_gtfsExtractPath);

            await ZipFile.ExtractToDirectoryAsync(
                _gtfsZipPath,
                _gtfsExtractPath,
                cancellationToken
            );

            logger.LogInformation("GTFS zip file extraction completed");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured while extracting GTFS");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!GtfsFileIsFresh())
            await FetchGtfsAsync(cancellationToken);
        else
            logger.LogInformation("GTFS file exists and is fresh. Fetching skipped.");

        if (!GtfsExtractedFilesAreFresh())
            await ExtractGtfsAsync(cancellationToken);
        else
            logger.LogInformation("GTFS extracted files exist. Extraction skipped.");

        while (!cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Task running at: {time}", DateTime.Now.ConvertToLocal());

            // schedule next fetch

            var now = DateTime.Now;
            var target = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                _options.DailyFetchHour,
                _options.DailyFetchMinute,
                _options.DailyFetchSecond
            );

            if (target < now)
                target = target.AddDays(1);

            logger.LogInformation("Next GTFS refetch scheduled at: {target}", target.ConvertToLocal());

            // parse and update state

            var parsedData = gtfsParserService.Parse();
            transitState.UpdateData(parsedData);

            // wait

            var delay = target - now;
            await Task.Delay(delay, cancellationToken);
        }
    }
}