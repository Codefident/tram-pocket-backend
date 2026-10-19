using CsvHelper.Configuration;

namespace TramPocketBackend.Models.Gtfs.Mappings;

public sealed class CalendarDateMappings : ClassMap<CalendarDate>
{
    public CalendarDateMappings()
    {
        Map(m => m.ServiceId).Name("service_id");
        Map(m => m.Date).Name("date");
        Map(m => m.ExceptionType).Name("exception_type");
    }
}