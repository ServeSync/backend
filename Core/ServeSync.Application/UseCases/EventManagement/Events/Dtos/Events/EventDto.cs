using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRegistrationInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

public class BasicEventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Introduction { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    
    public DateTime StartAt { get; init; }
    public DateTime EndAt { get; set; }
    
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public EventStatus CalculatedStatus { get; set; }
    public EventAddressDto Address { get; set; } = null!;
}

public class FlatEventDto : BasicEventDto
{
    public int Capacity { get; set; }
    public int Registered { get; set; }
    public int Rating { get; set; }
    
    public BasicEventActivityDto Activity { get; set; } = null!;
    public BasicOrganizationInEventDto RepresentativeOrganization { get; set; } = null!;
}

public class EventDetailDto : FlatEventDto
{
    public string Description { get; set; } = null!;
    public bool IsRegistered { get; set; }
    public bool IsAttendance { get; set; }
    public List<EventRoleDto> Roles { get; set; } = null!;
    public List<OrganizationInEventDto> Organizations { get; set; } = null!;
    public List<EventRegistrationDto> RegistrationInfos { get; set; } = null!;
    public List<EventAttendanceInfoDto> AttendanceInfos { get; set; } = null!;

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
        if (Status == EventStatus.Approved && StartAt <= dateTime && EndAt >= dateTime && AttendanceInfos.Any(x => x.StartAt <= dateTime && x.EndAt >= dateTime))
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
}

public class StudentAttendanceEventDto : FlatEventDto
{
    public string Role { get; set; } = null!;
    public double Score { get; set; }
    public DateTime AttendanceAt { get; set; }
}