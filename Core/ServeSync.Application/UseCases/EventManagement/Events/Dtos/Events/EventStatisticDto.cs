using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

public class EventStatisticDto
{
    public int Total { get; set; }
    public List<EventStatisticRecordDto> Data { get; set; } = null!;
}

public class EventStatisticRecordDto
{
    public EventStatus Status { get; set; }
    public int Count { get; set; }
}