using System.ComponentModel.DataAnnotations.Schema;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.SharedKernel.ValueObjects;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.EventAggregate.Entities;

public class Event : AuditableTenantAggregateRoot
{
    public string Name { get; private set; }
    public string Introduction { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime StartAt { get; private set; }
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
        StartAt = Guard.Range(startAt, nameof(StartAt), DateTime.UtcNow);
        SetEndAt(endAt);
        ActivityId = Guard.NotNull(activityId, nameof(ActivityId));
        Address = new EventAddress(fullAddress, longitude, latitude);
        
        Roles = new List<EventRole>();
        AttendanceInfos = new List<EventAttendanceInfo>();
        Organizations = new List<OrganizationInEvent>();
        RegistrationInfos = new List<EventRegistrationInfo>();
        
        AddDomainEvent(new NewEventCreatedDomainEvent(this));
    }

    internal void Update(string name, 
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
        StartAt = Guard.Range(startAt, nameof(StartAt), DateTime.UtcNow);
        SetEndAt(endAt);
        ActivityId = Guard.NotNull(activityId, nameof(ActivityId));
        Address = new EventAddress(fullAddress, longitude, latitude);
        
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    #region AttendanceInfo
    
    internal void AddAttendanceInfo(DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        CheckCanUpdateAttendanceInfo(dateTime);
        
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

    internal void UpdateAttendanceInfo(Guid id, DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        CheckCanUpdateAttendanceInfo(dateTime);
        
        var attendanceInfo = AttendanceInfos.FirstOrDefault(x => x.Id == id);
        if (attendanceInfo == null)
        {
            throw new EventAttendanceInfoNotFoundException(id);
        }
        
        if (AttendanceInfos.Any(x => x.Id != id && x.IsOverlapped(startAt, endAt)))
        {
            throw new EventAttendanceInfoOverlappedException(startAt, endAt);
        }
        
        attendanceInfo.Update(startAt, endAt);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void RemoveAttendanceInfo(Guid id, DateTime dateTime)
    {
        CheckCanUpdateAttendanceInfo(dateTime);
        
        var attendanceInfo = AttendanceInfos.FirstOrDefault(x => x.Id == id);
        if (attendanceInfo == null)
        {
            throw new EventAttendanceInfoNotFoundException(id);
        }
        
        AttendanceInfos.Remove(attendanceInfo);
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
    
    #endregion

    #region Role
    
    internal void AddRole(string name, string description, bool isNeedApprove, double score, int quantity, DateTime dateTime)
    {
        CheckCanUpdateRoleInfo(dateTime);
        
        if (Roles.Any(x => x.Name == name))
        {
            throw new EventRoleHasAlreadyExistException(name);
        }
        
        Roles.Add(new EventRole(name, description, isNeedApprove, score, quantity, Id));
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void UpdateRole(Guid id, string name, string description, bool isNeedApprove, double score, int quantity, DateTime dateTime)
    {
        CheckCanUpdateRoleInfo(dateTime);
        
        var role = Roles.FirstOrDefault(x => x.Id == id);
        if (role == null)
        {
            throw new EventRoleNotFoundException(id);
        }
        
        role.Update(name, description, isNeedApprove, score, quantity);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    internal void RemoveRole(Guid id, DateTime dateTime)
    {
        CheckCanUpdateRoleInfo(dateTime);
        
        var role = Roles.FirstOrDefault(x => x.Id == id);
        if (role == null)
        {
            throw new EventRoleNotFoundException(id);
        }
        
        Roles.Remove(role);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    #endregion

    #region Organization
    
    internal void AddOrganization(Guid organizationId, string role, DateTime dateTime)
    {
        CheckCanUpdateOrganizationInfo(dateTime);
        
        if (Organizations.Any(x => x.OrganizationId == organizationId))
        {
            throw new OrganizationHasAlreadyAddedToEventException(organizationId, Id);
        }
        
        Organizations.Add(new OrganizationInEvent(organizationId, role, Id));
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    internal void UpdateOrganization(Guid id, Guid organizationId, string role, DateTime dateTime)
    {
        CheckCanUpdateOrganizationInfo(dateTime);
        
        var organizationInEvent = Organizations.FirstOrDefault(x => x.Id == id);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(id);
        }
        
        organizationInEvent.Update(organizationId, role);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void RemoveOrganization(Guid id, DateTime dateTime)
    {
        CheckCanUpdateOrganizationInfo(dateTime);
        
        var organizationInEvent = Organizations.FirstOrDefault(x => x.Id == id);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(id);
        }
        
        Organizations.Remove(organizationInEvent);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void AddOrganizationRepresentative(Guid organizationId, Guid representativeId, string role, DateTime dateTime)
    {
        CheckCanUpdateOrganizationInfo(dateTime);
        
        var organizationInEvent = Organizations.FirstOrDefault(x => x.OrganizationId == organizationId);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(Id, organizationId);
        }
        
        organizationInEvent.AddRepresentative(representativeId, role);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    internal void UpdateOrganizationRepresentative(Guid organizationId, Guid id, Guid representativeId, string role, DateTime dateTime)
    {
        CheckCanUpdateOrganizationInfo(dateTime);
        
        var organizationInEvent = Organizations.FirstOrDefault(x => x.OrganizationId == organizationId);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(Id, organizationId);
        }
        
        organizationInEvent.UpdateRepresentative(id, representativeId, role);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    internal void RemoveOrganizationRepresentative(Guid organizationId, Guid id, DateTime dateTime)
    {
        CheckCanUpdateOrganizationInfo(dateTime);
        
        var organizationInEvent = Organizations.FirstOrDefault(x => x.OrganizationId == organizationId);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(Id, organizationId);
        }
        
        organizationInEvent.RemoveRepresentative(id);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    internal void SetRepresentativeOrganization(Guid representativeOrganizationId, DateTime dateTime)
    {
        CheckCanUpdateOrganizationInfo(dateTime);
        
        var organizationInEvent = Organizations.FirstOrDefault(x => x.OrganizationId == representativeOrganizationId);
        if (organizationInEvent == null)
        {
            throw new OrganizationHasNotAddedToEventException(Id, representativeOrganizationId);
        }
        
        RepresentativeOrganizationId = Guard.NotNull(organizationInEvent.Id, nameof(RepresentativeOrganizationId));
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    #endregion

    #region RegistrationInfo
    
    internal void AddRegistrationInfo(DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        if (dateTime >= StartAt || GetCurrentStatus(dateTime) == EventStatus.Expired)
        {
            throw new EventRegistrationInfoCannotBeAddedException();
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
    
    internal void UpdateRegistrationInfo(Guid id, DateTime startAt, DateTime endAt, DateTime dateTime)
    {
        var registrationInfo = RegistrationInfos.FirstOrDefault(x => x.Id == id);
        if (registrationInfo == null)
        {
            throw new EventRegistrationInfoNotFoundException(id, Id);
        }

        if (registrationInfo.StartAt <= dateTime || GetCurrentStatus(dateTime) == EventStatus.Expired)
        {
            throw new EventRegistrationInfoCannotBeUpdatedException(id);
        }
        
        if (RegistrationInfos.Any(x => x.Id != id && x.IsOverlapped(startAt, endAt)))
        {
            throw new EventRegistrationInfoOverlappedException();
        }
        
        registrationInfo.Update(startAt, endAt);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }
    
    internal void RemoveRegistrationInfo(Guid id, DateTime dateTime)
    {
        var registrationInfo = RegistrationInfos.FirstOrDefault(x => x.Id == id);
        if (registrationInfo == null)
        {
            throw new EventRegistrationInfoNotFoundException(id, Id);
        }
        
        if (registrationInfo.StartAt <= dateTime || GetCurrentStatus(dateTime) == EventStatus.Expired)
        {
            throw new EventRegistrationInfoCannotBeUpdatedException(id);
        }
        
        RegistrationInfos.Remove(registrationInfo);
        AddDomainEvent(new EventUpdatedDomainEvent(Id));
    }

    #endregion
    
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

    public void Approve(DateTime dateTime)
    {
        var startTime = RegistrationInfos.Min(x => x.StartAt);
        if (Status == EventStatus.Pending && dateTime < startTime)
        {
            Status = EventStatus.Approved;
            AddDomainEvent(new EventUpdatedDomainEvent(Id));
        }
        else
        {
            throw new EventCanNotBeApprovedException();
        }
    }

    public void Reject()
    {
        if (Status != EventStatus.Pending)
        {
            throw new EventCanNotBeRejectedException(Id);
        }
        
        Status = EventStatus.Rejected;
        AddDomainEvent(new EventRejectedDomainEvent(this));
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
    
    public EventStatus GetStatus(DateTime dateTime)
    {
        var currentStatus = GetCurrentStatus(dateTime);
        if (currentStatus == EventStatus.Pending || currentStatus == EventStatus.Expired)
        {
            return EventStatus.Pending;
        }
        else if (currentStatus == EventStatus.Happening || currentStatus == EventStatus.Attendance)
        {
            return EventStatus.Happening;
        }
        else if (currentStatus == EventStatus.Upcoming || currentStatus == EventStatus.Registration || currentStatus == EventStatus.ClosedRegistration)
        {
            return EventStatus.Upcoming;
        }

        return currentStatus;
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
        else if (Status == EventStatus.Pending && RegistrationInfos.Any(x => x.StartAt <= dateTime))
        {
            return EventStatus.Expired;
        }
        
        return Status;
    }

    private void CheckCanUpdateRegistrationInfo(DateTime dateTime)
    {
        if (RegistrationInfos.Any(x => x.StartAt <= dateTime))
        {
            throw new EventRegistrationInfoCannotBeUpdatedException(Id);
        }
    }
    
    private void CheckCanUpdateRoleInfo(DateTime dateTime)
    {
        if (RegistrationInfos.Any(x => x.StartAt <= dateTime))
        {
            throw new EventRoleCanNotBeUpdatedException(Id);
        }
    }
    
    private void CheckCanUpdateAttendanceInfo(DateTime dateTime)
    {
        if (StartAt <= dateTime || Status == EventStatus.Approved)
        {
            throw new EventAttendanceInfoCanNotBeUpdatedException(Id);
        }
    }
    
    private void CheckCanUpdateOrganizationInfo(DateTime dateTime)
    {
        if (StartAt <= dateTime || Status == EventStatus.Approved)
        {
            throw new EventOrganizationCanNotBeUpdatedException(Id);
        }
    }

    private Event()
    {
        
    }
}