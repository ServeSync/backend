using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

public class EventActivity : Entity
{
    public string Name { get; private set; }
    public double MinScore { get; private set; }
    public double MaxScore { get; private set; }
    public Guid EventCategoryId { get; private set; }
    public EventCategory? EventCategory { get; private set; }
    
    internal EventActivity(string name, double minScore, double maxScore, Guid eventCategoryId)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        MinScore = Guard.Range(minScore, nameof(MinScore), 0);
        MaxScore = Guard.Range(maxScore, nameof(MaxScore), minScore);
        EventCategoryId = Guard.NotNull(eventCategoryId, nameof(EventCategoryId));
    }
    
    private EventActivity()
    {
        
    }
}