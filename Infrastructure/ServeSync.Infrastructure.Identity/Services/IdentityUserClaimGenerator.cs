using System.Security.Claims;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Services;

public class IdentityUserClaimGenerator
{
    public static IEnumerable<Claim> Generate(ApplicationUser user)
    {
        var claims = new List<Claim>()
        {
            new (AppClaim.UserId, user.Id),
            new (AppClaim.UserName, user.UserName),
            new (AppClaim.Email, user.Email),
            new (AppClaim.ReferenceId, user.ExternalId!.ToString())
        };
        
        var tenant = user.Tenants.FirstOrDefault();
        if (tenant != null)
        {
            claims.Add(new Claim(AppClaim.TenantId, tenant.TenantId.ToString()));
        }

        return claims;
    }
}