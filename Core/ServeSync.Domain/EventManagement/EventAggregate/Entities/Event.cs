using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.SharedKernel.ValueObjects;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class Event : AggregateRoot
{
    public string Name { get; private set; }
    public string Introduction { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    
    public EventType Type { get; private set; }
    public EventAddress Address { get; private set; }
    
    public Guid ActivityId { get; private set; }
    public EventActivity? Activity { get; private set; }
    
    public List<EventRole> Roles { get; private set; }
    public List<EventAttendanceInfo> AttendanceInfos { get; private set; }
    public List<OrganizationInEvent> Organizations { get; private set; }

    internal Event(
        string name,
        string introduction,
        string description,
        string imageUrl,
        EventType type,
        DateTime startAt,
        DateTime endAt,
        Guid activityId,
        string fullAddress, 
        double longitude, 
        double latitude)
    {
        Name = Guard.NotNullOrWhiteSpace(name, nameof(Name));
        Introduction = Guard.NotNullOrWhiteSpace(introduction, nameof(Introduction));
        Description = Guard.NotNullOrWhiteSpace(description, nameof(Description));
        ImageUrl = Guard.NotNullOrWhiteSpace(imageUrl, nameof(ImageUrl));
        Type = type;
        StartAt = Guard.NotNull(startAt, nameof(StartAt));
        EndAt = Guard.Range(endAt, nameof(EndAt), startAt);
        ActivityId = Guard.NotNull(activityId, nameof(ActivityId));
        Address = new EventAddress(fullAddress, longitude, latitude);
        
        Roles = new List<EventRole>();
        AttendanceInfos = new List<EventAttendanceInfo>();
        Organizations = new List<OrganizationInEvent>();
    }

    private Event()
    {
        
    }
}