using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ServeSync.API.Common.Enums;
using ServeSync.Application.Common;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Authorization;

public class ServeSyncAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _defaultPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _defaultPolicyProvider.GetDefaultPolicyAsync();

    public ServeSyncAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(HasRoleAttribute.Prefix, StringComparison.OrdinalIgnoreCase))
        {
            var policyBuilder = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .AddRequirements(new RoleAuthorizationRequirement(policyName.Split(".").Last()));
            
            return Task.FromResult(policyBuilder.Build());
        }
        
        if (policyName.StartsWith(AppPermissions.Default, StringComparison.OrdinalIgnoreCase))
        {
            var policyBuilder = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionAuthorizationRequirement(policyName));
            
            return Task.FromResult(policyBuilder.Build());
        }
        
        if (policyName.StartsWith(EventAccessControlAttribute.Prefix, StringComparison.OrdinalIgnoreCase))
        {
            var policyBuilder = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new EventAccessControlRequirement(
                    (EventSourceAccessControl)Enum.Parse(typeof(EventSourceAccessControl), policyName.Split(".").Last(),
                        true)));
            
            return Task.FromResult(policyBuilder.Build());
        }

        return _defaultPolicyProvider.GetPolicyAsync(policyName);
    }
}