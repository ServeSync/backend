using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.ValueObjects;
using ServeSync.Domain.EventManagement.SharedKernel.ValueObjects;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;

public class EventCollaborationRequest : AuditableAggregateRoot
{
    public string Name { get; private set; }
    public string Introduction { get; private set; }
    public string Description { get; private set; }
    public int Capacity { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public EventType Type {get; private set; }    
    public CollaborationRequestStatus Status { get; private set; }
    public EventAddress Address { get; private set; }
    public EventOrganizationInfo Organization { get; private set; } 
    public EventOrganizationContactInfo OrganizationContact { get; private set; }
    
    public Guid ActivityId { get; private set; }
    public EventActivity? Activity { get; private set; }
    
    public Guid? EventId { get; private set; }

    internal EventCollaborationRequest(
        string name,
        string introduction,
        string description,
        int capacity,
        string imageUrl,
        DateTime startAt,
        DateTime endAt,
        EventType eventType,
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
        Name = Guard.NotNullOrWhiteSpace(name, nameof(Name));
        Introduction = Guard.NotNullOrWhiteSpace(introduction, nameof(Introduction));
        Description = Guard.NotNullOrWhiteSpace(description, nameof(Description));
        Capacity = Guard.Range(capacity, nameof(Capacity), 0);
        ImageUrl = Guard.NotNullOrWhiteSpace(imageUrl, nameof(ImageUrl));
        StartAt = Guard.NotNull(startAt, nameof(StartAt));
        SetEndAt(endAt);
        Type = Guard.NotNull(eventType, nameof(EventType));
        ActivityId = Guard.NotNull(activityId, nameof(ActivityId));
        Address = new EventAddress(fullAddress, longitude, latitude);

        if (organizationEmail == organizationContactEmail)
        {
            throw new ResourceInvalidDataException("Organization email and contact email can not be the same");
        }
        
        Organization = new EventOrganizationInfo(
            organizationName,
            organizationDescription,
            organizationEmail,
            organizationPhoneNumber,
            organizationAddress,
            organizationImageUrl);
        OrganizationContact = new EventOrganizationContactInfo(
            organizationContactName, 
            organizationContactEmail,
            organizationContactPhoneNumber, 
            organizationContactGender, 
            organizationContactAddress,
            organizationContactBirth, 
            organizationContactPosition, 
            organizationContactImageUrl);
        Status = CollaborationRequestStatus.Pending;
    }

    internal void Approve(DateTime dateTime)
    {
        if (dateTime > StartAt.AddDays(-1))
        {
            throw new EventCollaborationRequestApproveTimeExpiredException(Id);
        }

        if (Status != CollaborationRequestStatus.Pending)
        {
            throw new EventCollaborationRequestNotPendingException(Id);
        }
        
        Status = CollaborationRequestStatus.Approved;
        AddDomainEvent(new EventCollaborationRequestApprovedDomainEvent(this));
    }

    internal void Reject(DateTime dateTime)
    {
        if (GetStatus(dateTime) == CollaborationRequestStatus.Pending)
        {
            Status = CollaborationRequestStatus.Rejected;
            AddDomainEvent(new EventCollaborationRequestRejectedDomainEvent(this));
        }
        else
        {
            throw new EventCollaborationRequestCanNotBeRejectedException(Id);
        }
    }
    
    public CollaborationRequestStatus GetStatus(DateTime dateTime)
    {
        if (Status == CollaborationRequestStatus.Pending && StartAt.AddDays(-1) <= dateTime)
        {
            return CollaborationRequestStatus.Expired;
        }
        
        return Status;
    }
    
    public void SetEventId(Guid eventId)
    {
        EventId = Guard.NotNull(eventId, nameof(EventId));
    }
    
    internal void SetEndAt(DateTime endAt)
    {
        if (endAt < StartAt.AddHours(1))
        {
            throw new EventHeldShorterException();
        }
        EndAt = Guard.Range(endAt, nameof(EndAt), StartAt);
    }
    
    private EventCollaborationRequest()
    {
        
    }
}