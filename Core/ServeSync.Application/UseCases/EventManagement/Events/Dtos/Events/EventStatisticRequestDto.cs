using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

public class EventStatisticRequestDto
{
    public RecurringFilterType? Type { get; set; }
}