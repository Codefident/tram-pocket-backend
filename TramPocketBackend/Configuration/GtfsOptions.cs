namespace TramPocketBackend.Configuration;

public class GtfsOptions
{
    public string DataPath { get; set; } = string.Empty;
    public string KrkTUrl { get; set; } = string.Empty;
    public string ExtractPath { get; set; } = string.Empty;
    public string ZipPath { get; set; } = string.Empty;
    public int DailyFetchHour { get; set; }
    public int DailyFetchMinute { get; set; }
    public int DailyFetchSecond { get; set; }
}