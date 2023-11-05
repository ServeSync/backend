using ServeSync.Domain.EventManagement.EventCategoryAggregate.Enums;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

public class EventCategory : AggregateRoot
{
    public string Name { get; private set; }
    public double Index { get; private set; }
    public EventCategoryType Type { get; private set; }
    public List<EventActivity> Activities { get; private set; }

    internal EventCategory(string name, double index, EventCategoryType type)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        Activities = new List<EventActivity>();
        Index = index;
        Type = type;
    }

    internal void AddActivity(string name, double minScore, double maxScore)
    {
        var activity = new EventActivity(name, minScore, maxScore, Id, Activities.Count);
        Activities.Add(activity);                
    }
    
    private EventCategory()
    {
        Activities = new List<EventActivity>();
    }
}