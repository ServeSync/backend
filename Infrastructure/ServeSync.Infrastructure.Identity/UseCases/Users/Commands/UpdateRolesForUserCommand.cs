using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Commands;

public class UpdateRolesForUserCommand : ICommand
{
    public string UserId { get; set; }
    public Guid TenantId { get; set; }
    public IEnumerable<string> RoleIds { get; set; }
    
    public UpdateRolesForUserCommand(string userId, Guid tenantId, IEnumerable<string> roleIds)
    {
        UserId = userId;
        TenantId = tenantId;
        RoleIds = roleIds;
    }
}