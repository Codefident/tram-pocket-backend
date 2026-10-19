namespace TramPocketBackend.Utils;

public static class GtfsUtils
{
    public static string CreateStopTripKey(string stopId, string tripId)
    {
        return $"{stopId}_{tripId}";
    }
}