namespace TramPocketBackend.Models.Gtfs;

public record Route
{
    public string RouteId { get; set; } = string.Empty;
    public string AgencyId { get; set; } = string.Empty;
    public string RouteShortName { get; set; } = string.Empty;
    public string RouteLongName { get; set; } = string.Empty;
    public string RouteDesc { get; set; } = string.Empty;
    public int RouteType { get; set; }
    public string RouteUrl { get; set; } = string.Empty;
    public string RouteColor { get; set; } = string.Empty;
    public string RouteTextColor { get; set; } = string.Empty;
}