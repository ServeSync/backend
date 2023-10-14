using System.ComponentModel.DataAnnotations;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

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
    
    [Required]
    [MinLength(1)]
    public List<EventRoleCreateDto> Roles { get; set; } = null!;
    
    [Required]
    [MinLength(1)]
    public List<EventAttendanceInfoCreateDto> AttendanceInfos { get; set; } = null!;
    
    [Required]
    [MinLength(1)]
    public List<OrganizationInEventCreateDto> Organizations { get; set; } = null!;
}

public class EventAttendanceInfoCreateDto
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}

public class EventRoleCreateDto
{
    [Required]
    [MinLength(5)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MinLength(10)]
    public string Description { get; set; } = null!;
    
    [Required]
    public bool IsNeedApprove { get; set; }
    
    [Required]
    public double Score { get; set; }
    
    [Required]
    public int Quantity { get; set; }
}

public class OrganizationInEventCreateDto
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Role { get; set; } = null!;
    
    public List<OrganizationRepInEventCreateDto> OrganizationReps { get; set; } = null!;
}

public class OrganizationRepInEventCreateDto
{
    [Required]
    public Guid OrganizationRepId { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Role { get; set; } = null!;
}