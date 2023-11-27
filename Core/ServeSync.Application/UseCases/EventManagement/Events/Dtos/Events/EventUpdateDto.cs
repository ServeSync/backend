using System.ComponentModel.DataAnnotations;
using ServeSync.Application.Common.Validations;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRegistrationInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

public class EventUpdateDto
{
    [MinLength(10)]
    public string? Name { get; set; }
    
    [MinLength(10)]
    public string? Introduction { get; set; }
    
    [MinLength(256)]
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    
    public EventType? Type { get; set; }
    public Guid? ActivityId { get; set; }
    public Guid? RepresentativeOrganizationId { get; set; }
    public EventAddressDto? Address { get; set; }
    
    [MinLength(1)]
    public List<EventRoleUpdateDto>? Roles { get; set; }
    
    [MinLength(1)]
    public List<EventAttendanceInfoUpdateDto>? AttendanceInfos { get; set; }
    
    [MinLength(1)]
    public List<OrganizationInEventUpdateDto>? Organizations { get; set; }
    
    [MinLength(1)]
    public List<EventRegistrationUpdateDto>? RegistrationInfos { get; set; }
}