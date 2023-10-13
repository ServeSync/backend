using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos;

public class OrganizationRepInEventCreateDto
{
    [Required]
    public Guid OrganizationRepId { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Role { get; set; } = null!;
}