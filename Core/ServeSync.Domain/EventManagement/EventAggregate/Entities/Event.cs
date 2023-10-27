using System.ComponentModel.DataAnnotations.Schema;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.SharedKernel.ValueObjects;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class Event : AuditableAggregateRoot
{
    public string Name { get; private set; }
    public string Introduction { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime StartAt { get; private init; }
    public DateTime EndAt { get; private set; }
    
    public EventType Type { get; private set; }
    public EventStatus Status { get; private set; }
    public EventAddress Address { get; private set; }
    
    public Guid ActivityId { get; private set; }
    public EventActivity? Activity { get; private set; }

    public Guid? RepresentativeOrganizationId { get; private set; } = null!;
    public OrganizationInEvent? RepresentativeOrganization { get; private set; }
    
    public List<EventRole> Roles { get; private set; }
    public List<EventAttendanceInfo> AttendanceInfos { get; private set; }
    public List<OrganizationInEvent> Organizations { get; private set; }
    public List<EventRegistrationInfo> RegistrationInfos { get; private set; }

    internal Event(
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
        Name = Guard.NotNullOrWhiteSpace(name, nameof(Name));
        Introduction = Guard.NotNullOrWhiteSpace(introduction, nameof(Introduction));
        Description = Guard.NotNullOrWhiteSpace(description, nameof(Description));
        ImageUrl = Guard.NotNullOrWhiteSpace(imageUrl, nameof(ImageUrl));
        Type = type;
        Status = EventStatus.Pending;
        StartAt = Guard.Range(startAt, nameof(StartAt), DateTime.Now);
        SetEndAt(endAt);
        ActivityId = Guard.NotNull(activityId, nameof(ActivityId));
        Address = new EventAddress(fullAddress, longitude, latitude);
        
        Roles = new List<EventRole>();
        AttendanceInfos = new List<EventAttendanceInfo>();
        Organizations = new List<OrganizationInEvent>();
        RegistrationInfos = new List<EventRegistrationInfo>();
        
        AddDomainEvent(new NewEventCreatedDomainEvent(this));
    }
    
    internal void AddAttendanceInfo(DateTime startAt, DateTime endAt)
    {
        var attendanceInfo = new EventAttendanceInfo(startAt, endAt, Id);
        
        if (StartAt >= endAt || EndAt <= startAt)
        {
            throw new EventAttendanceInfoOutOfEventTimeException();
        }
        
        if (AttendanceInfos.Any(x => x.IsOverlapped(startAt, endAt)))
        {
            throw new EventAttendanceInfoOverlappedException(startAt, endAt);
        }
        
        AttendanceInfos.Add(attendanceInfo);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void SetAttendanceQrCodeUrl(Guid id, string qrCodeUrl)
    {
        var attendanceInfo = AttendanceInfos.FirstOrDefault(x => x.Id == id);
        if (attendanceInfo == null)
        {
            throw new EventAttendanceInfoNotFoundException(id);
        }
        
        attendanceInfo.SetQrCodeUrl(qrCodeUrl);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void AddRole(string name, string description, bool isNeedApprove, double score, int quantity)
    {
        if (Roles.Any(x => x.Name == name))
        {
            throw new EventRoleHasAlreadyExistException(name);
        }
        
        Roles.Add(new EventRole(name, description, isNeedApprove, score, quantity, Id));
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    internal void AddOrganization(Guid organizationId, string role)
    {
        if (Organizations.Any(x => x.OrganizationId == organizationId))
        {
            throw new OrganizationHasAlreadyAddedToEventException(organizationId, Id);
        }
        
        Organizations.Add(new OrganizationInEvent(organizationId, role, Id));
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void AddOrganizationRepresentative(Guid organizationId, Guid representativeId, string role)
    {
        var organizationInEvent = Organizations.FirstOrDefault(x => x.OrganizationId == organizationId);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(Id, organizationId);
        }
        
        organizationInEvent.AddRepresentative(representativeId, role);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void SetRepresentativeOrganization(Guid representativeOrganizationId)
    {
        var organizationInEvent = Organizations.FirstOrDefault(x => x.OrganizationId == representativeOrganizationId);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(Id, representativeOrganizationId);
        }
        
        RepresentativeOrganizationId = Guard.NotNull(organizationInEvent.Id, nameof(RepresentativeOrganizationId));
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void AddRegistrationInfo(DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        if (!CanUpdateRegistrationInfo(dateTime))
        {
            throw new EventRegistrationInfoCannotBeUpdatedException(Id);    
        }
        
        if (startAt >= StartAt || endAt >= StartAt)
        {
            throw new EventRegistrationOverlappedWithEventException();
        }

        if (RegistrationInfos.Any(x => x.IsOverlapped(startAt, endAt)))
        {
            throw new EventRegistrationInfoOverlappedException();
        }
        
        RegistrationInfos.Add(new EventRegistrationInfo(startAt, endAt, Id));
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void Cancel(DateTime dateTime)
    {
        if (Status == EventStatus.Approved && StartAt > dateTime)
        {
            Status = EventStatus.Cancelled;
            AddDomainEvent(new EventCancelledDomainEvent(this));
            AddDomainEvent(new EventUpdatedDomainEvent(Id));
        }
        else
        {
            throw new EventCanNotBeCancelledException();
        }
    }

    public void Approve()
    {
        Status = EventStatus.Approved;
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    public bool IsInAttendArea(double longitude, double latitude)
    {
        return Address.DistanceTo(new EventAddress(Address.FullAddress, longitude, latitude)) <= 200;
    }

    public bool CanRegister(DateTime dateTime)
    {
        return GetCurrentStatus(dateTime) == EventStatus.Registration;
    }
    
    public bool CanAttendance(DateTime dateTime)
    {
        return GetCurrentStatus(dateTime) == EventStatus.Attendance;
    }
    
    public bool ValidateCode(string token, DateTime dateTime)
    {
        return AttendanceInfos.Any(x => x.ValidateCode(token, dateTime));
    }

    public void SetEndAt(DateTime endAt)
    {
        if (endAt < StartAt.AddHours(1))
        {
            throw new EventHeldShorterException();
        }
        EndAt = Guard.Range(endAt, nameof(EndAt), StartAt);
    }
    
    public EventStatus GetCurrentStatus(DateTime dateTime)
    {
        if (Status == EventStatus.Approved && StartAt <= dateTime && EndAt >= dateTime && AttendanceInfos.Any(x => x.CanAttendance(dateTime)))
        {
            return EventStatus.Attendance;
        }
        else if (Status == EventStatus.Approved && StartAt <= dateTime && EndAt >= dateTime)
        {
            return EventStatus.Happening;
        }
        else if (Status == EventStatus.Approved && StartAt >= dateTime && RegistrationInfos.Any(x => dateTime >= x.StartAt && dateTime <= x.EndAt))
        {
            return EventStatus.Registration;
        }
        else if (Status == EventStatus.Approved && StartAt >= dateTime && RegistrationInfos.All(x => dateTime >= x.EndAt))
        {
            return EventStatus.ClosedRegistration;
        }
        else if (Status == EventStatus.Approved && StartAt >= dateTime)
        {
            return EventStatus.Upcoming;
        }
        else if (Status == EventStatus.Approved && EndAt <= dateTime)
        {
            return EventStatus.Done;
        }
        else if (Status == EventStatus.Pending && StartAt <= dateTime)
        {
            return EventStatus.Expired;
        }
        
        return Status;
    }

    private bool CanUpdateRegistrationInfo(DateTime dateTime)
    {
        if (RegistrationInfos.Any(x => x.StartAt <= dateTime))
        {
            return false;
        }

        return true;
    }

    private Event()
    {
        
    }
}