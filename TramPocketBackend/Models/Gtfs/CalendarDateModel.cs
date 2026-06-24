namespace TramPocketBackend.Models.Gtfs;

public record CalendarDateModel
{
    public string ServiceId { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public int ExceptionType { get; set; }
}