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

    Task<Event> UpdateAsync(
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
        DateTime dateTime);

    Event AddAttendanceInfo(Event @event, DateTime startAt, DateTime endAt, DateTime dateTime);

    Event RemoveAttendanceInfo(Event @event, Guid id, DateTime dateTime);
    
    Event UpdateAttendanceInfo(Event @event, Guid id, DateTime startAt, DateTime endAt, DateTime dateTime);

    Event AddRole(Event @event, string name, string description, bool isNeedApprove, double score, int quantity, DateTime dateTime);
    
    Event UpdateRole(Event @event, Guid id, string name, string description, bool isNeedApprove, double score, int quantity, DateTime dateTime);

    Event RemoveRole(Event @event, Guid id, DateTime dateTime);

    Task<Event> AddDefaultRoleAsync(Event @event, int quantity, DateTime dateTime);
    
    Event AddOrganization(Event @event, EventOrganization organization, string role, DateTime dateTime);
    
    Event RemoveOrganization(Event @event, Guid id, DateTime dateTime);
    
    Event UpdateOrganization(Event @event, Guid id, EventOrganization organization, string role, DateTime dateTime);

    Event SetRepresentativeOrganization(Event @event, Guid organizationId, DateTime dateTime);
    
    Event AddRepresentative(Event @event, EventOrganization organization, EventOrganizationContact representative, string role, DateTime dateTime);
    
    Event UpdateRepresentative(Event @event, EventOrganization organization, Guid id, EventOrganizationContact representative, string role, DateTime dateTime);
    
    Event RemoveRepresentative(Event @event, EventOrganization organization, Guid id, DateTime dateTime);
    
    Event AddRegistrationInfo(Event @event, DateTime startAt, DateTime endAt, DateTime dateTime);
    
    Event RemoveRegistrationInfo(Event @event, Guid id, DateTime dateTime);
    
    Event UpdateRegistrationInfo(Event @event, Guid id, DateTime startAt, DateTime endAt, DateTime dateTime);
    
    Event SetAttendanceQrCodeUrl(Event @event, Guid id, string qrCodeUrl);

    Event Reject(Event @event);

    Event CancelEvent(Event @event, DateTime dateTime);
    
    Event ApproveEvent(Event @event, DateTime dateTime);
}