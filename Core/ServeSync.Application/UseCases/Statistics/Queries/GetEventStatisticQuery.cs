using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.Statistics.Dtos;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetEventStatisticQuery : IQuery<EventStatisticDto>
{
    public RecurringFilterType? Type { get; set; }
    
    public GetEventStatisticQuery(RecurringFilterType? type)
    {
        Type = type;
    }
}