using System.Globalization;
using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.Common.Helpers;

public static class DateTimeHelper
{
    public static (DateTime startAt, DateTime endAt) GetByRecurringType(RecurringFilterType type, DateTime dateTime)
    {
        switch (type)
        {
            case RecurringFilterType.Today:
                return (dateTime.Date, dateTime.Date.AddDays(1).AddTicks(-1));
            
            case RecurringFilterType.ThisWeek:
                var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                dateTime = dateTime.AddDays(firstDayOfWeek - dateTime.DayOfWeek + 1);
                
                return (dateTime.Date, dateTime.Date.AddDays(7).AddTicks(-1));
            
            case RecurringFilterType.ThisMonth:
                var startOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);
                
                return (startOfMonth, endOfMonth);
            
            case RecurringFilterType.ThisYear:
                var startOfYear = new DateTime(dateTime.Year, 1, 1);
                var endOfYear = startOfYear.AddYears(1).AddTicks(-1);
                
                return (startOfYear, endOfYear);
        }
        
        return (dateTime, dateTime);
    }
    
    public static DateTime CurrentTimeZone(this DateTime dateTime)
    {
        var vietnamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, vietnamZone);
        
        return localDateTime;
    }
}