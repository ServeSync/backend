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

    internal void AddActivity(string name, double minScore, double maxScore)
    {
        var activity = new EventActivity(name, minScore, maxScore, Id);
        Activities.Add(activity);
    }
    
    private EventCategory()
    {
        Activities = new List<EventActivity>();
    }
}