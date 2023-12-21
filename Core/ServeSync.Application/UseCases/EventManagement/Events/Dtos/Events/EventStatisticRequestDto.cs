using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

public class EventStatisticRequestDto
{
    public RecurringFilterType? Type { get; set; }
}

public class EventStudentStatisticRequestDto
{
    public int NumberOfRecords { get; set; } = 7;
    public TimeType Type { get; set; } = TimeType.Date;
}