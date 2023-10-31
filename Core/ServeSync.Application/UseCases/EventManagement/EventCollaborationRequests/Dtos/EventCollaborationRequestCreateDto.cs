using System.ComponentModel.DataAnnotations;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

public class EventCollaborationRequestCreateDto
{
    [Required]
    [MinLength(10)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(128)]
    public string Introduction { get; set; } = null!;

    [Required]
    [MinLength(256)]
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

    public EventOrganizationInfoDto EventOrganizationInfo { get; set; } = null!;

    public EventOrganizationContactInfoDto EventOrganizationContactInfo { get; set; } = null!; 
}
