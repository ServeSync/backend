using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;

public class GetAllEventActivityQuery : IQuery<IEnumerable<EventActivityDto>>
{
    public Guid EventCategoryId { get; set; }
    
    public GetAllEventActivityQuery(Guid eventCategoryId)
    {
        EventCategoryId = eventCategoryId;
    }
}