using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Commands;

public class GrantPermissionToRoleCommand : ICommand
{
    public string RoleId { get; set; }
    public Guid PermissionId { get; set; }
}