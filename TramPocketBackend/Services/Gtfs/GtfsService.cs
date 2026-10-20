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

    private bool GtfsFileExists() => File.Exists(_gtfsZipPath);

    private async Task FetchGtfsAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching GTFS from {url}...", _options.KrkTUrl);

        try
        {
            // fetch data and stream it directly into the file
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

    private async Task RefreshGtfsAsync(CancellationToken cancellationToken)
    {
        await FetchGtfsAsync(cancellationToken);
        await ExtractGtfsAsync(cancellationToken);
        transitState.UpdateData(gtfsParserService.Parse());
    }

    private TimeSpan GetTaskDelay()
    {
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

        return target - now;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await RefreshGtfsAsync(cancellationToken);
            await Task.Delay(GetTaskDelay(), cancellationToken);
        }
    }
}