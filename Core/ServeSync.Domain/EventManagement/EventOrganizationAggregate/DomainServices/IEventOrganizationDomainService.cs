using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;

public interface IEventOrganizationDomainService
{
    Task<EventOrganization> CreateAsync(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address, 
        OrganizationStatus status = OrganizationStatus.Pending);

    EventOrganizationContact AddContact(
        EventOrganization eventOrganization,
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl,
        bool? gender, 
        DateTime? birth, 
        string? address, 
        string? position,
        OrganizationStatus status = OrganizationStatus.Pending);
    
    Task<EventOrganization> UpdateInfoAsync(
        EventOrganization eventOrganization,
        string name, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address);
    
    Task<EventOrganization> UpdateContactAsync(
        EventOrganization eventOrganization,
        Guid eventOrganizationContactId, 
        string name,
        string phoneNumber,
        string imageUrl,
        bool? gender,
        DateTime? birth,
        string? address,
        string? position);
    
    Task DeleteAsync(EventOrganization eventOrganization);
    
    Task DeleteContactAsync(EventOrganization eventOrganization, Guid eventOrganizationContactId);

    void ProcessInvitation(OrganizationInvitation invitation);
}