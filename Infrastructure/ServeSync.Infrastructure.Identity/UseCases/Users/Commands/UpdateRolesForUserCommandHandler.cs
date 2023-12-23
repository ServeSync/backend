using Microsoft.Extensions.Logging;
using ServeSync.Application.Common;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Commands;

public class UpdateRolesForUserCommandHandler : ICommandHandler<UpdateRolesForUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRolesForUserCommandHandler> _logger;
    
    public UpdateRolesForUserCommandHandler(IUserRepository userRepository, 
        IRoleRepository roleRepository, 
        IUnitOfWork unitOfWork,
        ILogger<UpdateRolesForUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(UpdateRolesForUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId, "Roles.Role");
        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }
        
        if (user.Tenants.All(x => x.TenantId != request.TenantId))
        {
            throw new UserNotInTenantException(request.UserId, request.TenantId);
        }
        
        var roles = await GetRoles(request);
        
        UpdateRoles(user, roles, request.TenantId);
        
        await _unitOfWork.CommitAsync();
        _logger.LogInformation("Roles for user {UserId} has been updated!", request.UserId);
    }
    
    private void UpdateRoles(ApplicationUser user, IList<ApplicationRole> roles, Guid tenantId)
    {
        user.ClearRole(tenantId);
        
        foreach (var role in roles)
        {
            if (AppRole.All.Contains(role.Name!))
            {
                throw new DefaultRoleAccessDeniedException(role.Name!);
            }
            
            user.GrantRole(role.Id, tenantId);
        }
    }

    private async Task<IList<ApplicationRole>> GetRoles(UpdateRolesForUserCommand request)
    {
        request.RoleIds = request.RoleIds.Distinct().ToList();
        
        var roles = await _roleRepository.FindByIncludedIdsAsync(request.RoleIds);
        if (roles.Count != request.RoleIds.Count())
        {
            var notFoundRoleId = request.RoleIds.First(x => !roles.Select(r => r.Id).Contains(x));
            throw new RoleNotFoundException(notFoundRoleId);
        }
        
        return roles;
    }
}