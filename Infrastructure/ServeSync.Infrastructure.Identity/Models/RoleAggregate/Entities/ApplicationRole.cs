using Microsoft.AspNetCore.Identity;
using ServeSync.Application.Common;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.DomainEvents;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

public partial class ApplicationRole : IdentityRole
{
    public List<RolePermission> Permissions { get; set; }
        
    public ApplicationRole(string roleName) : base(roleName)
    {
        if (IsDefaultRole(roleName))
        {
            throw new DefaultRoleAccessDeniedException(Name);    
        }
        
        Permissions = new List<RolePermission>();
    }

    private ApplicationRole()
    {
        
    }

    public void ClearPermission()
    {
        if (IsDefaultRole(Name))
        {
            throw new DefaultRoleAccessDeniedException(Name);    
        }
        
        Permissions.Clear();
        AddDomainEvent(new PermissionForRoleUpdatedDomainEvent(Name));
    }
    
    public void GrantPermission(Guid permissionId)
    {
        if (HasPermission(permissionId))
        {
            throw new PermissionHasAlreadyGrantedToRoleException(Id, permissionId);
        }
        
        Permissions.Add(new RolePermission(Id, permissionId));
        AddDomainEvent(new PermissionForRoleUpdatedDomainEvent(Name));
    }
    
    public void UnGrantPermission(Guid permissionId)
    {
        var permission = Permissions.FirstOrDefault(x => x.PermissionId == permissionId);
        if (permission == null)
        {
            throw new PermissionHasNotGrantedToRoleException(Id, permissionId);
        }
        
        Permissions.Remove(permission);
        AddDomainEvent(new PermissionForRoleUpdatedDomainEvent(Name));
    }

    public void Update(string name)
    {
        if (IsDefaultRole(Name) || IsDefaultRole(name))
        {
            throw new DefaultRoleAccessDeniedException(name);    
        }
        
        AddDomainEvent(new RoleNameUpdatedDomainEvent(Name));
        Name = name;
    }

    public void Destroy()
    {
        if (IsDefaultRole(Name))
        {
            throw new DefaultRoleAccessDeniedException(Name);    
        }
    }

    private bool HasPermission(Guid permissionId)
    {
        return Permissions.Any(x => x.PermissionId == permissionId);
    }

    private bool IsDefaultRole(string name)
    {
        return string.Equals(name, AppRole.Admin, StringComparison.CurrentCultureIgnoreCase)
               || string.Equals(name, AppRole.EventOrganization, StringComparison.CurrentCultureIgnoreCase)
               || string.Equals(name, AppRole.StudentAffair, StringComparison.CurrentCultureIgnoreCase)
               || string.Equals(name, AppRole.Student, StringComparison.CurrentCultureIgnoreCase)
               || string.Equals(name, AppRole.EventOrganizer, StringComparison.CurrentCultureIgnoreCase);
    }
}