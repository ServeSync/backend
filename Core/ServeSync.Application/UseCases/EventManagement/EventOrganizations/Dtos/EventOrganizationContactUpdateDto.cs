using System.ComponentModel.DataAnnotations;
using ServeSync.Application.Common.Validations;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationContactUpdateDto
{
    [Required]
    [MinLength(5)]
    public string Name { get; set; } = null!;
    
    public bool? Gender { get; set; }
    
    [LessThanCurrentDate(nameof(Birth))]
    public DateTime? Birth { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    
    public string? Address { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = null!;
    
    public string? Position { get; set; }

    [Required]
    public Guid EventOrganizationId { get; set; }
}