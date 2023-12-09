using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
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

    public async Task<Event> UpdateAsync(
        Event @event,
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
        double latitude,
        DateTime dateTime)
    {
        if (@event.RegistrationInfos.Any(x => x.StartAt < dateTime))
        {
            throw new EventCanNotBeUpdatedException(@event.Id);
        }
        
        if (@event.ActivityId != activityId)
        {
            await CheckValidEventActivityExistedAsync(activityId);
        }
        
        @event.Update(
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

    public Event AddAttendanceInfo(Event @event, DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        @event.AddAttendanceInfo(startAt, endAt, dateTime);
        return @event;
    }

    public Event RemoveAttendanceInfo(Event @event, Guid id, DateTime dateTime)
    {
        @event.RemoveAttendanceInfo(id, dateTime);
        return @event;
    }

    public Event UpdateAttendanceInfo(Event @event, Guid id, DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        @event.UpdateAttendanceInfo(id, startAt, endAt, dateTime);
        return @event;
    }

    public async Task<Event> AddRoleAsync(Event @event, string name, string description, bool isNeedApprove, double score, int quantity, DateTime dateTime)
    {
        var activity = await _eventActivityRepository.FindByIdAsync(@event.ActivityId);
        if (activity!.MaxScore < score || score < activity.MinScore)
        {
            throw new EventActivityScoreOutOfRangeException(activity.Id, score);
        }
        
        @event.AddRole(name, description, isNeedApprove, score, quantity, dateTime);
        return @event;
    }

    public async Task<Event> UpdateRoleAsync(Event @event, Guid id, string name, string description, bool isNeedApprove, double score, int quantity, DateTime dateTime)
    {
        var activity = await _eventActivityRepository.FindByIdAsync(@event.ActivityId);
        if (activity!.MaxScore < score || score < activity.MinScore)
        {
            throw new EventActivityScoreOutOfRangeException(activity.Id, score);
        }
        
        @event.UpdateRole(id, name, description, isNeedApprove, score, quantity, dateTime);
        return @event;
    }

    public Event RemoveRole(Event @event, Guid id, DateTime dateTime)
    {
        @event.RemoveRole(id, dateTime);
        return @event;
    }

    public async Task<Event> AddDefaultRoleAsync(Event @event, int quantity, DateTime dateTime)
    {
        var activity = await _eventActivityRepository.FindByIdAsync(@event.ActivityId);
        
        @event.AddRole(
            "Người tham dự",
            "Người tham dự sự kiện",
            false, 
            (activity!.MaxScore + activity.MinScore) / 2,
            quantity,
            dateTime);
        
        return @event;
    }

    public Event AddOrganization(Event @event, EventOrganization organization, string role, DateTime dateTime)
    {
        if (organization.Status != OrganizationStatus.Active)
        {
            throw new EventOrganizationNotActiveException(organization.Id);
        }
        
        @event.AddOrganization(organization.Id, role, dateTime);
        return @event;
    }

    public Event RemoveOrganization(Event @event, Guid id, DateTime dateTime)
    {
        @event.RemoveOrganization(id, dateTime);
        return @event;
    }

    public Event UpdateOrganization(Event @event, Guid id, EventOrganization eventOrganization, string role, DateTime dateTime)
    {
        @event.UpdateOrganization(id, eventOrganization.Id, role, dateTime);
        return @event;
    }

    public Event SetRepresentativeOrganization(Event @event, Guid organizationId, DateTime dateTime)
    {
        @event.SetRepresentativeOrganization(organizationId, dateTime);
        return @event;
    }

    public Event AddRepresentative(
        Event @event, 
        EventOrganization organization, 
        EventOrganizationContact representative,
        string role,
        DateTime dateTime)
    {
        if (representative.Status != OrganizationStatus.Active)
        {
            throw new EventOrganizationContactNotActiveException(representative.Id);
        }
        
        if (representative.EventOrganizationId != organization.Id)
        {
            throw new EventOrganizationContactDoesNotBelongToOrganizationException(organization.Id, representative.Id);
        }
        
        @event.AddOrganizationRepresentative(organization.Id, representative.Id, role, dateTime);
        return @event;
    }

    public Event UpdateRepresentative(Event @event, EventOrganization organization, Guid id, EventOrganizationContact representative, string role, DateTime dateTime)
    {
        if (representative.EventOrganizationId != organization.Id)
        {
            throw new EventOrganizationContactDoesNotBelongToOrganizationException(organization.Id, representative.Id);
        }
        
        @event.UpdateOrganizationRepresentative(organization.Id, id, representative.Id, role, dateTime);
        return @event;
    }

    public Event RemoveRepresentative(Event @event, EventOrganization organization, Guid id, DateTime dateTime)
    {
        @event.RemoveOrganizationRepresentative(organization.Id, id, dateTime);
        return @event;
    }

    public Event AddRegistrationInfo(Event @event, DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        @event.AddRegistrationInfo(startAt, endAt, dateTime);
        return @event;
    }

    public Event RemoveRegistrationInfo(Event @event, Guid id, DateTime dateTime)
    {
        @event.RemoveRegistrationInfo(id, dateTime);
        return @event;
    }

    public Event UpdateRegistrationInfo(Event @event, Guid id, DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        @event.UpdateRegistrationInfo(id, startAt, endAt, dateTime);
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