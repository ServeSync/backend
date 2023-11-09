using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;

public class EventOrganizationDomainService : IEventOrganizationDomainService
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    
    public EventOrganizationDomainService(IEventOrganizationRepository eventOrganizationRepository)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
    }
    
    public async Task<EventOrganization> CreateAsync(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address)
    {
        var eventOrganization = new EventOrganization(name, email, phoneNumber, imageUrl, description, address);
        
        return eventOrganization;
    }

    public EventOrganization AddContact(
        EventOrganization eventOrganization,
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        bool? gender, 
        DateTime? birth, 
        string? address, 
        string? position)
    {
        eventOrganization.AddContact(name, email, phoneNumber, imageUrl, gender, birth, address, position);
        
        // _eventOrganizationRepository.Update(eventOrganization);
        return eventOrganization;
    }

    public async Task<EventOrganization> UpdateBaseInfoAsync(
        EventOrganization eventOrganization, 
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl,
        string? description = null, 
        string? address = null)
    {
        if (eventOrganization.Email != email)
        {
            await CheckDuplicateEmailAsync(email);
        }
        
        eventOrganization.UpdateBaseInfo(name, email, phoneNumber, imageUrl, description, address);
        _eventOrganizationRepository.Update(eventOrganization);
        return eventOrganization;
    }

    private async Task CheckDuplicateEmailAsync(string email)
    {
        if (await _eventOrganizationRepository.AnyAsync(new EventOrganizationByEmailSpecification(email))) 
        {
            throw new EventOrganizationEmailException(email);
        }   
    }
}