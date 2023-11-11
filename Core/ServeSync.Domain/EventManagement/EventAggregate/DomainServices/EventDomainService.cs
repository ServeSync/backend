using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Domain.EventManagement.EventAggregate.DomainServices;

public class EventDomainService : IEventDomainService
{
    private readonly IEventRepository _eventRepository;
    
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository;
    
    public EventDomainService(
        IEventRepository eventRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository)
    {
        _eventRepository = eventRepository;
        _eventActivityRepository = eventActivityRepository;
    }
    
    public async Task<Event> CreateAsync(
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
        await CheckValidEventActivityExistedAsync(activityId);
        var @event = new Event(
            name,
            introduction,
            description,
            imageUrl,
            type,
            startAt,
            endAt,
            activityId,
            fullAddress,
            longitude,
            latitude);
        
        return @event;
    }

    public Event AddAttendanceInfo(Event @event, DateTime startAt, DateTime endAt)
    {
        @event.AddAttendanceInfo(startAt, endAt);
        return @event;
    }

    public Event AddRole(Event @event, string name, string description, bool isNeedApprove, double score, int quantity)
    {
        @event.AddRole(name, description, isNeedApprove, score, quantity);
        return @event;
    }

    public async Task<Event> AddDefaultRoleAsync(Event @event, int quantity)
    {
        var activity = await _eventActivityRepository.FindByIdAsync(@event.ActivityId);
        
        @event.AddRole(
            "Người tham dự",
            "Người tham dự sự kiện",
            false, 
            activity!.MaxScore,
            quantity);
        
        return @event;
    }

    public Event AddOrganization(Event @event, EventOrganization organization, string role)
    {
        @event.AddOrganization(organization.Id, role);
        return @event;
    }

    public Event SetRepresentativeOrganization(Event @event, Guid organizationId)
    {
        @event.SetRepresentativeOrganization(organizationId);
        return @event;
    }

    public Event AddRepresentative(
        Event @event, 
        EventOrganization organization, 
        EventOrganizationContact representative,
        string role)
    {
        if (representative.EventOrganizationId != organization.Id)
        {
            throw new EventOrganizationContactDoesNotBelongToOrganizationException(organization.Id, representative.Id);
        }
        
        @event.AddOrganizationRepresentative(organization.Id, representative.Id, role);
        return @event;
    }

    public Event AddRegistrationInfo(Event @event, DateTime startAt, DateTime endAt, DateTime currentDateTime)
    {
        @event.AddRegistrationInfo(startAt, endAt, currentDateTime);
        return @event;
    }

    public Event SetAttendanceQrCodeUrl(Event @event, Guid id, string qrCodeUrl)
    {
        @event.SetAttendanceQrCodeUrl(id, qrCodeUrl);
        return @event;
    }

    public Event Reject(Event @event)
    {
        @event.Reject();
        return @event;
    }

    private async Task CheckValidEventActivityExistedAsync(Guid activityId)
    {
        if (!await _eventActivityRepository.AnyAsync(new EventActivityFromEventCategorySpecification(activityId)))
        {
            throw new EventActivityNotFoundException(activityId);
        }
    }

    public Event CancelEvent(Event @event, DateTime dateTime)
    {
        @event.Cancel(dateTime);
        
        return @event;
    }

    public Event ApproveEvent(Event @event, DateTime dateTime)
    {
        @event.Approve(dateTime);
        
        return @event;
    }
}