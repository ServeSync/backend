using Microsoft.AspNetCore.Identity;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

public partial class ApplicationRole : IdentityRole
{
    public List<RolePermission> Permissions { get; set; }
        
    public ApplicationRole(string roleName) : base(roleName)
    {
        if (IsAdminRole(roleName))
        {
            throw new AdminRoleAccessDeniedException();    
        }
        
        Permissions = new List<RolePermission>();
    }

    private ApplicationRole()
    {
        
    }

    public void GrantPermission(ApplicationPermission permission)
    {
        if (HasPermission(permission.Id))
        {
            throw new PermissionHasAlreadyGrantedToRoleException(Id, permission.Id);
        }
        
        Permissions.Add(new RolePermission(Id, permission.Id));
    }
    
    public void UnGrantPermission(ApplicationPermission permission)
    {
        if (!HasPermission(permission.Id))
        {
            throw new PermissionHasNotGrantedToRoleException(Id, permission.Id);
        }
        
        Permissions.Add(new RolePermission(Id, permission.Id));
    }

    public void Update(string name)
    {
        if (IsAdminRole(Name) || IsAdminRole(name))
        {
            throw new AdminRoleAccessDeniedException();    
        }
        
        Name = name;
    }

    public void Destroy()
    {
        if (IsAdminRole(Name))
        {
            throw new AdminRoleAccessDeniedException();    
        }
    }

    private bool HasPermission(Guid permissionId)
    {
        return Permissions.Any(x => x.PermissionId == permissionId);
    }

    private bool IsAdminRole(string name)
    {
        return string.Equals(name, AppRole.Admin, StringComparison.CurrentCultureIgnoreCase);
    }
}