using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
public record EventOrganizationInfoDto
{
    [Required]
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    
    public string? Address { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = null!;
}