using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;

public class GetAllEventActivityQuery : IQuery<IEnumerable<EventActivityDto>>
{
    public Guid? EventCategoryId { get; set; }
    public EventCategoryType? Type { get; set; }
    
    public GetAllEventActivityQuery(Guid? eventCategoryId = default, EventCategoryType? type = default)
    {
        EventCategoryId = eventCategoryId;
        Type = type;
    }
}