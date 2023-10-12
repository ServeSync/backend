using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;

public interface IEventCategoryDomainService
{
    Task<EventCategory> CreateAsync(string name);

    EventCategory AddActivity(EventCategory eventCategory, string name, double minScore, double maxScore);
}