using System.ComponentModel.DataAnnotations;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

public class RoleCreateDto
{
    [Required] 
    public string Name { get; set; } = null!;
}