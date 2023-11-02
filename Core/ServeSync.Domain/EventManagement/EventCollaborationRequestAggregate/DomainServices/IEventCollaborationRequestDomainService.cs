using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainServices;

public interface IEventCollaborationRequestDomainService 
{
    Task<EventCollaborationRequest> CreateAsync(
        string name,
        string introduction,
        string description,
        int capacity,
        string imageUrl,
        DateTime startAt,
        DateTime endAt,
        EventType type,
        Guid activityId,
        string fullAddress, 
        double longitude, 
        double latitude,
        string organizationName, 
        string? organizationDescription, 
        string organizationEmail, 
        string organizationPhoneNumber, 
        string? organizationAddress,
        string organizationImageUrl,
        string organizationContactName, 
        string organizationContactEmail, 
        string organizationContactPhoneNumber, 
        bool? organizationContactGender, 
        string? organizationContactAddress,
        DateTime? organizationContactBirth, 
        string? organizationContactPosition, 
        string organizationContactImageUrl);

    EventCollaborationRequest Approve(EventCollaborationRequest eventCollaborationRequest, DateTime dateTime);
}