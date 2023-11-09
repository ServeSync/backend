using System.Text.Json.Serialization;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;
public class GetAllEventCategoryQuery : IQuery<IEnumerable<EventCategoryDto>>
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventCategoryType? Type { get; set; }
    
    public GetAllEventCategoryQuery(EventCategoryType? type)
    {
        Type = type;
    }
}