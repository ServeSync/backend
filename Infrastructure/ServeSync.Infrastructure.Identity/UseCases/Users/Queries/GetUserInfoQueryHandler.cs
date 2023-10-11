using Microsoft.AspNetCore.Identity;
using ServeSync.Application.Identity;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, UserInfoDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityService _identityService;

    public GetUserInfoQueryHandler(
        ICurrentUser currentUser, 
        UserManager<ApplicationUser> userManager,
        IIdentityService identityService)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    public async Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_currentUser.Id);
        if (user == null)
        {
            throw new UserNotFoundException(_currentUser.Id);
        }

        var roles = await _identityService.GetRolesAsync(user.Id);
        var permissions = await _identityService.GetPermissionsForUserAsync(user.Id);
        
        return new UserInfoDto()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Roles = roles,
            Permissions = permissions
        };
    }
}