using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos;

public class OrganizationInEventCreateDto
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Role { get; set; } = null!;
    
    public List<OrganizationRepInEventCreateDto> OrganizationReps { get; set; } = null!;
}