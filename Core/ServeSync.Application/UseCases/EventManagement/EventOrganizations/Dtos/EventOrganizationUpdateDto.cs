using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationUpdateDto
{
    [Required]
    [MinLength(5)]
    public string Name { get; set; }
    
    public string? Description { get; set; }

    [Required] 
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    
    public string? Address { get; set; }
    
    [Required]
    public string ImageUrl { get; set; }
}