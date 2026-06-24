using CsvHelper.Configuration;
using TramPocketBackend.Models.Gtfs;

namespace TramPocketBackend.Mappings.Gtfs;

public sealed class CalendarDateMap : ClassMap<CalendarDateModel>
{
    public CalendarDateMap()
    {
        Map(m => m.ServiceId).Name("service_id");
        Map(m => m.Date).Name("date");
        Map(m => m.ExceptionType).Name("exception_type");
    }
}