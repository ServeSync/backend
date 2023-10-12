using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;

public class EventCategoryDomainService : IEventCategoryDomainService
{
    private readonly IEventCategoryRepository _eventCategoryRepository;
    
    public EventCategoryDomainService(IEventCategoryRepository eventCategoryRepository)
    {
        _eventCategoryRepository = eventCategoryRepository;
    }
    
    public async Task<EventCategory> CreateAsync(string name)
    {
        var eventCategory = new EventCategory(name);

        await _eventCategoryRepository.InsertAsync(eventCategory);
        return eventCategory;
    }

    public EventCategory AddActivity(EventCategory eventCategory, string name, double minScore, double maxScore)
    {
        eventCategory.AddActivity(name, minScore, maxScore);
        // _eventCategoryRepository.Update(eventCategory);

        return eventCategory;
    }
}