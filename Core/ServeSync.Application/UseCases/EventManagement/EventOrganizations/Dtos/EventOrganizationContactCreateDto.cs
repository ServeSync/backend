using System.ComponentModel.DataAnnotations;
using ServeSync.Application.Common.Validations;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

public class EventOrganizationContactCreateDto
{
    [Required] 
    [MinLength(5)] 
    public string Name { get; set; } = null!;
    
    [LessThanCurrentDate("Birth")]
    public DateTime? Birth { get; set; }

    [EmailAddress] 
    public string Email { get; set; } = null!;

    [Phone] 
    public string PhoneNumber { get; set; } = null!;
    
    public bool? Gender { get; set; }
    public string? Address { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? Position { get; set; }
}