using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

public class EventCategory : AggregateRoot
{
    public string Name { get; private set; }
    public List<EventActivity> Activities { get; private set; }

    internal EventCategory(string name)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Activities = new List<EventActivity>();
    }
    
    private EventCategory()
    {
        Activities = new List<EventActivity>();
    }
}