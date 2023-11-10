using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;

public interface IEventOrganizationDomainService
{
    Task<EventOrganization> CreateAsync(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address);

    EventOrganization AddContact(
        EventOrganization eventOrganization,
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl,
        bool? gender, 
        DateTime? birth, 
        string? address, 
        string? position);
    
    Task<EventOrganization> UpdateInfoAsync(
        EventOrganization eventOrganization,
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address);
    
    void Delete(EventOrganization eventOrganization);
}