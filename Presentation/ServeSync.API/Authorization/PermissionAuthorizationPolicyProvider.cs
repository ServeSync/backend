using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Authorization;

public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _defaultPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _defaultPolicyProvider.GetDefaultPolicyAsync();

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(Permissions.Default, StringComparison.OrdinalIgnoreCase))
        {
            return _defaultPolicyProvider.GetPolicyAsync(policyName);
        }

        var policyBuilder = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .AddRequirements(new PermissionAuthorizationRequirement(policyName));
            
        return Task.FromResult(policyBuilder.Build());

    }
}