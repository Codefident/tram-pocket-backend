namespace TramPocketBackend.Extensions;

public static class DateTimeExtensions
{
    extension(DateTime dateTime)
    {
        public string ConvertToLocal() => dateTime.ToString("dd-MM-yyyy HH:mm:ss");
        public string ConvertToGtfsCalendarDate() => dateTime.ToString("yyyyMMdd");
    }
}