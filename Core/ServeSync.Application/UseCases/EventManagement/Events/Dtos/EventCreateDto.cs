using System.ComponentModel.DataAnnotations;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos;

public class EventCreateDto
{
    [Required]
    [MinLength(10)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MinLength(10)]
    public string Introduction { get; set; } = null!;
    
    [Required]
    [MinLength(256)]
    public string Description { get; set; } = null!;
    
    [Required]
    public string ImageUrl { get; set; } = null!;
    
    [Required]
    public DateTime StartAt { get; set; }
    
    [Required]
    public DateTime EndAt { get; set; }
    public EventType Type { get; set; }
    public Guid ActivityId { get; set; }
    public Guid RepresentativeOrganizationId { get; set; }

    public EventAddressDto Address { get; set; } = null!;
    public List<EventRoleCreateDto> Roles { get; set; } = null!;
    public List<EventAttendanceInfoCreateDto> AttendanceInfos { get; set; } = null!;
    public List<OrganizationInEventCreateDto> Organizations { get; set; } = null!;
}