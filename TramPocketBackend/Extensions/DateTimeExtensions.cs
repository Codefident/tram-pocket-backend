namespace TramPocketBackend.Extensions;

public static class DateTimeExtensions
{
    extension(DateTime dateTime)
    {
        public string ConvertToLocal() => dateTime.ToString("dd-MM-yyyy HH:mm:ss");
    }
}