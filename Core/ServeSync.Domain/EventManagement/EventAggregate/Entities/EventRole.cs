using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class EventRole : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsNeedApprove { get; private set; }
    public double Score { get; private set; }
    public int Quantity { get; private set; }
    
    public Guid EventId { get; private set; }
    public Event? Event { get; private set; }

    internal EventRole(string name, string description, bool isNeedApprove, double score, int quantity, Guid eventId)
    {
        Name = Guard.NotNullOrWhiteSpace(name, nameof(Name));
        Description = Guard.NotNullOrWhiteSpace(description, nameof(Description));
        IsNeedApprove = isNeedApprove;
        Score = Guard.Range(score, nameof(Score), 0);
        Quantity = Guard.Range(quantity, nameof(Quantity), 1);
        EventId = Guard.NotNull(eventId, nameof(EventId));
    }

    private EventRole()
    {
        
    }
}