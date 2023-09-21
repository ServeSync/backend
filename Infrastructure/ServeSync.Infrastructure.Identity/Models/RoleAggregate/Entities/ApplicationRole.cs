using Microsoft.AspNetCore.Identity;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.DomainEvents;
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

    public void ClearPermission()
    {
        if (IsAdminRole(Name))
        {
            throw new AdminRoleAccessDeniedException();    
        }
        
        Permissions.Clear();
        AddDomainEvent(new PermissionForRoleUpdatedDomainEvent(Id));
    }
    
    public void GrantPermission(Guid permissionId)
    {
        if (HasPermission(permissionId))
        {
            throw new PermissionHasAlreadyGrantedToRoleException(Id, permissionId);
        }
        
        Permissions.Add(new RolePermission(Id, permissionId));
        AddDomainEvent(new PermissionForRoleUpdatedDomainEvent(Id));
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