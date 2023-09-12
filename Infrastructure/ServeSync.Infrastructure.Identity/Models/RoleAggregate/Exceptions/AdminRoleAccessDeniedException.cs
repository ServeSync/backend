using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

public class AdminRoleAccessDeniedException : ResourceAccessDeniedException
{
    public AdminRoleAccessDeniedException() 
        : base($"Can not create, update or delete {AppRole.Admin} role!", ErrorCodes.AdminRoleAccessDenied)
    {
    }
}