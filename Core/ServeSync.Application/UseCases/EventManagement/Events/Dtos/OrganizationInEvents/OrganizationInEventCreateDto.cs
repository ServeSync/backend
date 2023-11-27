using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;

public class OrganizationInEventCreateDto
{
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Role { get; set; } = null!;
    
    [Required]
    [MinLength(1)]
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
