using System.ComponentModel.DataAnnotations.Schema;
using ServeSync.Application.Common;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

public class RoleDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    [NotMapped]
    public bool IsDefault => AppRole.All.Contains(Name);
}