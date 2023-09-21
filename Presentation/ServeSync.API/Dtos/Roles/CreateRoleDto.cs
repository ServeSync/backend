using System.ComponentModel.DataAnnotations;

namespace ServeSync.API.Dtos.Roles;

public class CreateRoleDto
{
    [Required]
    public string Name { get; set; }
}