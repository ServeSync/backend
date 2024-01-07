using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.UseCases.Statistics.Dtos;

public class EventStatisticRequestDto
{
    public RecurringFilterType? Type { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
}

public class EventStudentStatisticRequestDto
{
    public int NumberOfRecords { get; set; } = 7;
    public TimeType Type { get; set; } = TimeType.Date;
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
}