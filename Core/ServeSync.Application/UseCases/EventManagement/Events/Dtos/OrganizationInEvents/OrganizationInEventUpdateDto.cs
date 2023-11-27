using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;

public class OrganizationInEventUpdateDto
{
    public Guid? Id { get; set; }
    
    [Required]
    public Guid OrganizationId { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Role { get; set; } = null!;
    
    [Required]
    [MinLength(1)]
    public List<OrganizationRepInEventUpdateDto> OrganizationReps { get; set; } = null!;
}

public class OrganizationRepInEventUpdateDto
{
    public Guid? Id { get; set; }
    
    [Required]
    public Guid OrganizationRepId { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Role { get; set; } = null!;
}
