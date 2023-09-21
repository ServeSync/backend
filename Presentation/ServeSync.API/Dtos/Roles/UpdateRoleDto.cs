using System.ComponentModel.DataAnnotations;

namespace ServeSync.API.Dtos.Roles;

public class UpdateRoleDto
{
    [Required]
    public string Name { get; set; }
}