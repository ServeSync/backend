using System.ComponentModel.DataAnnotations;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.ValueObjects;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

public class EventCollaborationRequestDto
{
    [Required]
    [MinLength(10)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(128)]
    public string Introduction { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public int Capacity { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public DateTime StartAt { get; set; }

    [Required]
    public DateTime EndAt { get; set; }

    [Required]
    public EventType EventType { get; set; }

    [Required]
    public Guid ActivityId { get; set; }

    [Required]
    public EventAddressDto Address { get; set; } = null!;

    public EventOrganizationInfo EventOrganizationInfo { get; set; } = null!;

    public EventOrganizationContactInfo EventOrganizationContactInfo { get; set; } = null!; 
}
