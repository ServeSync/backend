using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Domain.EventManagement.EventAggregate.DomainServices;

public interface IEventDomainService
{
    Task<Event> CreateAsync(
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
        double latitude);

    Event AddAttendanceInfo(Event @event, DateTime startAt, DateTime endAt);

    Event AddRole(Event @event, string name, string description, bool isNeedApprove, double score, int quantity);

    Task<Event> AddDefaultRoleAsync(Event @event, int quantity);
    
    Event AddOrganization(Event @event, EventOrganization organization, string role);

    Event SetRepresentativeOrganization(Event @event, Guid organizationId);
    
    Event AddRepresentative(Event @event, EventOrganization organization, EventOrganizationContact representative, string role);
    
    Event AddRegistrationInfo(Event @event, DateTime startAt, DateTime endAt, DateTime dateTime);
    
    Event SetAttendanceQrCodeUrl(Event @event, Guid id, string qrCodeUrl);

    Event CancelEvent(Event @event, DateTime dateTime);
    
    Event ApproveEvent(Event @event, DateTime dateTime);
}