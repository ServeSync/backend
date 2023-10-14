using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
public record EventOrganizationContactInfoDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string PhoneNumber { get; set; } = null!;
    public bool? Gender { get; set; }
    public string? Address { get; set; }
    public DateTime? Birth { get; set; }
    public string? Position { get; set; }
    [Required]
    public string ImageUrl { get; set; } = null!;
}