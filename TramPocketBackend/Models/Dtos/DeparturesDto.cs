namespace TramPocketBackend.Models.Dtos;

public record DeparturesDto
{
    public string StopName { get; init; } = string.Empty;
    public List<Departure> Departures { get; init; } = [];
}