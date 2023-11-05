using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

public class EventCategoryDto
{
    public Guid Id { get; set; }
    public string Name { set; get; } = null!;
    public EventCategoryType Type { get; set; }
}