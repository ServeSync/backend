using System.ComponentModel.DataAnnotations;
using ServeSync.Application.Common.Validations;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
public record EventOrganizationContactInfoDto
{
    [Required]
    [MinLength(5)]
    public string Name { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    
    public bool? Gender { get; set; }
    
    public string? Address { get; set; }
    
    [LessThanCurrentDate(nameof(Birth))]
    public DateTime? Birth { get; set; }
    
    public string? Position { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = null!;
}