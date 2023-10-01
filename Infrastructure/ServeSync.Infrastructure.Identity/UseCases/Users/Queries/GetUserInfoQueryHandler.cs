using Microsoft.AspNetCore.Identity;
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

    public GetUserInfoQueryHandler(
        ICurrentUser currentUser, 
        UserManager<ApplicationUser> userManager)
    {
        _currentUser = currentUser;
        _userManager = userManager;
    }
    
    public async Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_currentUser.Id);
        if (user == null)
        {
            throw new UserNotFoundException(_currentUser.Id);
        }

        var roles = await _userManager.GetRolesAsync(user);
        
        return new UserInfoDto()
        {
            Id = user.Id,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Roles = roles
        };
    }
}