using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Application.UseCases.Statistics.Queries;

namespace ServeSync.Application.Common.Helpers;

public static class StatisticHelper
{
    public static List<EventStudentStatisticDto> EnrichResult(List<EventStudentStatisticDto> result,
        TimeType timeType,
        TimeType calculatedTimeType,
        DateTime? endAt,
        int numberOfRecords)
    {
        var dateTime = timeType is TimeType.Custom
            ? endAt!.Value
            : DateTime.UtcNow.CurrentTimeZone();
        
        switch (calculatedTimeType)
        {
            case TimeType.Date:
                result.AddRange(Enumerable.Range(0, numberOfRecords)
                    .Select(offset => dateTime.AddDays(-offset))
                    .Where(dt => result.All(x => x.Day != dt.Day || x.Month != dt.Month || x.Year != dt.Year))
                    .Select(dt => new EventStudentStatisticDto
                    {
                        Name = dt.ToString("dd/MM/yyyy"),
                        Value = 0,
                        Day = dt.Day,
                        Month = dt.Month,
                        Year = dt.Year
                    }));
                break;
            
            case TimeType.Month:
                result.AddRange(Enumerable.Range(0, numberOfRecords)
                    .Select(offset => dateTime.AddMonths(-offset))
                    .Where(dt => result.All(x => x.Month != dt.Month || x.Year != dt.Year))
                    .Select(dt => new EventStudentStatisticDto
                    {
                        Name = $"{dt.Month}/{dt.Year}",
                        Value = 0,
                        Month = dt.Month,
                        Year = dt.Year
                    }).ToList());
                break;
            
            case TimeType.Year:
                result.AddRange(Enumerable.Range(0, numberOfRecords)
                    .Select(offset => dateTime.AddYears(-offset))
                    .Where(dt => result.All(x => x.Year != dt.Year))
                    .Select(dt => new EventStudentStatisticDto
                    {
                        Name = dt.Year.ToString(),
                        Value = 0,
                        Year = dt.Year
                    }));
                break;
        }

        return result.OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Day)
            .ToList();
    }
}

public struct StatisticConstant
{
    public const int DayInMonth = 28;
    public const int DayInYear = 365;
}