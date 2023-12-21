using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetEventStatisticQuery : IQuery<EventStatisticDto>
{
    public RecurringFilterType? Type { get; set; }
    
    public GetEventStatisticQuery(RecurringFilterType? type)
    {
        Type = type;
    }
}