using System.ComponentModel.DataAnnotations;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

public class RoleUpdateDto
{
    [Required]
    public string Name { get; set; }
}