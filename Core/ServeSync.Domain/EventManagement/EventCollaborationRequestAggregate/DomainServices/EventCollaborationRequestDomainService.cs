using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainServices;

public class EventCollaborationRequestDomainService : IEventCollaborationRequestDomainService
{
    private readonly IEventCollaborationRequestRepository _eventCollaborationRequestRepository;
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository;

    public EventCollaborationRequestDomainService(
        IEventCollaborationRequestRepository eventCollaborationRequestRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository)
    {
        _eventCollaborationRequestRepository = eventCollaborationRequestRepository;
        _eventActivityRepository = eventActivityRepository;
    }

    public async Task<EventCollaborationRequest> CreateAsync(
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
        string organizationContactImageUrl)
    {
        await CheckEventActivityExistedAsync(activityId);
        var eventCollaborationRequest = new EventCollaborationRequest(
            name, 
            introduction,
            description,
            capacity,
            imageUrl,
            startAt,
            endAt,
            type,
            activityId,
            fullAddress,
            longitude,
            latitude,
            organizationName,
            organizationDescription,
            organizationEmail,
            organizationPhoneNumber,
            organizationAddress,
            organizationImageUrl,
            organizationContactName,
            organizationContactEmail,
            organizationContactPhoneNumber,
            organizationContactGender,
            organizationContactAddress,
            organizationContactBirth,
            organizationContactPosition,
            organizationContactImageUrl);
        
        return eventCollaborationRequest;
    }

    public EventCollaborationRequest Approve(EventCollaborationRequest eventCollaborationRequest, DateTime dateTime)
    {
        eventCollaborationRequest.Approve(dateTime);

        return eventCollaborationRequest;
    }
    
    public EventCollaborationRequest Reject(EventCollaborationRequest eventCollaborationRequest, DateTime dateTime)
    {
        eventCollaborationRequest.Reject(dateTime);

        return eventCollaborationRequest;
    }


    private async Task CheckEventActivityExistedAsync(Guid activityId)
    {
        if (!await _eventActivityRepository.IsExistingAsync(activityId))
        {
            throw new EventActivityNotFoundException(activityId);
        }
    }
}