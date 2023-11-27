using Microsoft.AspNetCore.Identity;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

public partial class ApplicationUser : IdentityUser<string>
{
    public string FullName { get; private set; }
    public string AvatarUrl { get; private set; }
    public Guid? ExternalId { get; private set; }
    public List<RefreshToken> RefreshToken { get; set; }
    public List<UserInTenant> Tenants { get; set; }
    public List<ApplicationUserInRole> Roles { get; set; }

    public ApplicationUser(string fullname, string? avatarUrl = null, Guid? externalId = null)
    {
        Id = Guid.NewGuid().ToString();
        FullName = Guard.NotNullOrEmpty(fullname, nameof(FullName));
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? "https://static.thenounproject.com/png/5034901-200.png" : avatarUrl;
        ExternalId = externalId;
        RefreshToken = new List<RefreshToken>();
        Tenants = new List<UserInTenant>();
        Roles = new List<ApplicationUserInRole>();
    }

    public void UpdateFullName(string fullName)
    {
        FullName = Guard.NotNullOrEmpty(fullName, nameof(FullName));
    }

    public void SetAvatar(string avatar)
    {
        AvatarUrl = Guard.NotNullOrWhiteSpace(avatar, nameof(AvatarUrl));
    }

    public void AddRefreshToken(string accessTokenId, string refreshToken, DateTime expireAt)
    {
        if (GetRefreshToken(accessTokenId, refreshToken) != null)
        {
            throw new RefreshTokeHasAlreadyAddedException(refreshToken);
        }
        
        RefreshToken.Add(new RefreshToken(accessTokenId, refreshToken, expireAt, Id));
    }

    public void UseRefreshToken(string accessTokenId, string refreshToken)
    {
        if (CanRefreshToken(accessTokenId, refreshToken))
        {
            RefreshToken.Remove(GetRefreshToken(accessTokenId, refreshToken));
        }
    }

    public void GrantRole(string roleId, Guid tenantId)
    {
        if (Roles.Any(x => x.RoleId == roleId && x.TenantId == tenantId))
        {
            throw new ResourceAlreadyExistException("Role has already granted to user in this tenant!");
        }
        
        Roles.Add(new ApplicationUserInRole(Id, roleId, tenantId));
    }
    
    public void UnGrantRole(string roleId, Guid tenantId)
    {
        var role = Roles.FirstOrDefault(x => x.RoleId == roleId && x.TenantId == tenantId);

        if (role == null)
        {
            throw new ResourceNotFoundException("Role not found!");
        }

        Roles.Remove(role);
    }

    public void AddToTenant(string fullName, string avatarUrl, Guid tenantId, bool isOwner)
    {
        if (Tenants.Any(x => x.TenantId == tenantId))
        {
            throw new UserAlreadyInTenantException(Id, tenantId);
        }
        
        Tenants.Add(new UserInTenant(tenantId, Id, fullName, avatarUrl, isOwner));
    }
    
    public void UpdateProfileInTenant(string fullName, string avatarUrl, Guid tenantId)
    {
        var tenant = Tenants.FirstOrDefault(x => x.TenantId == tenantId);

        if (tenant == null)
        {
            throw new UserNotInTenantException(Id, tenantId);
        }

        tenant.Update(fullName, avatarUrl);
    }
    
    public void RemoveFromTenant(Guid tenantId)
    {
        var tenant = Tenants.FirstOrDefault(x => x.TenantId == tenantId);

        if (tenant == null)
        {
            throw new UserNotInTenantException(Id, tenantId);
        }

        Tenants.Remove(tenant);
    }

    private bool CanRefreshToken(string accessTokenId, string refreshToken)
    {
        var token = GetRefreshToken(accessTokenId, refreshToken);
        
        if (token == null)
        {
            throw new RefreshTokenNotFoundException(refreshToken);
        }

        if (token.IsExpire())
        {
            throw new RefreshTokenHasAlreadyExpireException(refreshToken);
        }

        return true;
    }

    private RefreshToken? GetRefreshToken(string accessTokenId, string refreshToken)
    {
        return RefreshToken.FirstOrDefault(x => x.AccessTokenId == accessTokenId && x.Value == refreshToken);
    }

    private ApplicationUser()
    {
        RefreshToken = new List<RefreshToken>();
    }
}