using System.Security.Claims;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Services;

public class IdentityUserClaimGenerator
{
    public static IEnumerable<Claim> Generate(ApplicationUser user, Guid? tenantId = null)
    {
        var claims = new List<Claim>()
        {
            new (AppClaim.UserId, user.Id),
            new (AppClaim.UserName, user.UserName),
            new (AppClaim.Email, user.Email),
            new (AppClaim.ReferenceId, user.ExternalId!.ToString())
        };

        if (tenantId != null)
        {
            if (user.Tenants.All(x => x.TenantId != tenantId.Value))
            {
                throw new UserNotInTenantException(user.Id, tenantId.Value);
            }
            
            claims.Add(new Claim(AppClaim.TenantId, tenantId.Value.ToString()));
        }
        else
        {
            var tenant = user.Tenants.FirstOrDefault();
            if (tenant != null)
            {
                claims.Add(new Claim(AppClaim.TenantId, tenant.TenantId.ToString()));
            }    
        }

        return claims;
    }
}