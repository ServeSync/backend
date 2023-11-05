using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

public class EventCategoryFilterRequestDto
{
    public EventCategoryType? Type { get; set; }
}