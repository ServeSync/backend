﻿using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Enums;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;

public class EventOrganizationDomainService : IEventOrganizationDomainService
{
    private readonly IEventOrganizationRepository _eventOrganizationRepository;
    private readonly IOrganizationInvitationRepository _organizationInvitationRepository;
    private readonly IBasicReadOnlyRepository<OrganizationRepInEvent, Guid> _organizationRepInEventRepository;
    
    public EventOrganizationDomainService(
        IEventOrganizationRepository eventOrganizationRepository,
        IOrganizationInvitationRepository organizationInvitationRepository,
        IBasicReadOnlyRepository<OrganizationRepInEvent, Guid> organizationRepInEventRepository)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _organizationInvitationRepository = organizationInvitationRepository;
        _organizationRepInEventRepository = organizationRepInEventRepository;
    }
    
    public async Task<EventOrganization> CreateAsync(
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        string? description, 
        string? address,
        OrganizationStatus status = OrganizationStatus.Pending)
    {
        await CheckDuplicateNameAsync(name);
        await CheckDuplicateEmailAsync(email);
        var eventOrganization = new EventOrganization(name, email, phoneNumber, imageUrl, description, address, status);
        
        return eventOrganization;
    }

    public EventOrganizationContact AddContact(
        EventOrganization eventOrganization,
        string name, 
        string email, 
        string phoneNumber, 
        string imageUrl, 
        bool? gender, 
        DateTime? birth, 
        string? address, 
        string? position,
        OrganizationStatus status = OrganizationStatus.Pending)
    {
        return eventOrganization.AddContact(name, email, phoneNumber, imageUrl, gender, birth, address, position, status);
    }

    public async Task<EventOrganization> UpdateInfoAsync(
        EventOrganization eventOrganization, 
        string name, 
        string phoneNumber, 
        string imageUrl,
        string? description = null, 
        string? address = null)
    {
        if (eventOrganization.Name != name)
        {
            await CheckDuplicateNameAsync(name);
        }
        
        eventOrganization.Update(name, phoneNumber, imageUrl, description, address);
        _eventOrganizationRepository.Update(eventOrganization);
        return eventOrganization;
    }

    public async Task<EventOrganization> UpdateContactAsync(
        EventOrganization eventOrganization, 
        Guid eventOrganizationContactId, 
        string name,
        string phoneNumber, 
        string imageUrl, 
        bool? gender = null, 
        DateTime? birth = null, 
        string? address = null,
        string? position = null)
    {
        eventOrganization.UpdateEventOrganizationContact(
            eventOrganizationContactId, 
            name, 
            phoneNumber, 
            imageUrl,
            gender, 
            birth, 
            address, 
            position);
        return eventOrganization;
    }

    public async Task DeleteAsync(EventOrganization eventOrganization)
    {
        var hasHostAnyEvent = await _eventOrganizationRepository.HasHostAnyEventAsync(eventOrganization.Id);
        
        if (hasHostAnyEvent)
        {
            throw new EventOrganizationHasHostedAnEventException(eventOrganization.Id);
        }
        
        eventOrganization.AddDomainEvent(new EventOrganizationDeletedDomainEvent(eventOrganization));
        _eventOrganizationRepository.Delete(eventOrganization);
    }

    public async Task DeleteContactAsync(EventOrganization eventOrganization, Guid eventOrganizationContactId)
    {
        var eventOrganizationContact = eventOrganization.Contacts.FirstOrDefault(x => x.Id == eventOrganizationContactId);
        
        if (eventOrganizationContact == null)
        {
            throw new EventOrganizationContactNotFoundException(eventOrganizationContactId);
        }
        
        var hasAnyEventBeenAttendedByContact = await _organizationRepInEventRepository.FindAsync(x => x.OrganizationRep == eventOrganizationContact);
        if (hasAnyEventBeenAttendedByContact != null)
        {
            throw new EventOrganizationContactHasAttendAnEventException(eventOrganizationContact.Id);
        }

        eventOrganization.DeleteEventOrganizationContact(eventOrganizationContactId);
    }

    public void ProcessInvitation(OrganizationInvitation invitation)
    {
        _organizationInvitationRepository.Delete(invitation);
    }

    private async Task CheckDuplicateEmailAsync(string email)
    {
        if (await _eventOrganizationRepository.AnyAsync(new EventOrganizationByEmailSpecification(email))) 
        {
            throw new EventOrganizationEmailException(email);
        }   
    }
    
    private async Task CheckDuplicateNameAsync(string name)
    {
        if (await _eventOrganizationRepository.AnyAsync(new EventOrganizationByNameSpecification(name))) 
        {
            throw new EventOrganizationNameException(name);
        }   
    }
}