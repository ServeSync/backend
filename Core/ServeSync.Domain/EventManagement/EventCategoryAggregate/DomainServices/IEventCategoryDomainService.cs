using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;

public interface IEventCategoryDomainService
{
    Task<EventCategory> CreateAsync(string name, double index, EventCategoryType type);

    EventCategory AddActivity(EventCategory eventCategory, string name, double minScore, double maxScore);
}