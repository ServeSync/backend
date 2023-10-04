﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.Caching.Interfaces;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

public class GetAllRoleForUserQueryHandler : IQueryHandler<GetAllRoleForUserQuery, IEnumerable<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserCacheManager _userCacheManager;
    private readonly ILogger<GetAllRoleForUserQueryHandler> _logger;
    
    public GetAllRoleForUserQueryHandler(
        IUserCacheManager userCacheManager,
        UserManager<ApplicationUser> userManager,
        ILogger<GetAllRoleForUserQueryHandler> logger)
    {
        _userManager = userManager;
        _userCacheManager = userCacheManager;
        _logger = logger;
    }
    
    public async Task<IEnumerable<string>> Handle(GetAllRoleForUserQuery request, CancellationToken cancellationToken)
    {
        var roles = await _userCacheManager.GetRolesAsync(request.UserId);
        if (roles == null)
        {
            _logger.LogInformation("[Get role] Cache missed!");
            
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }
            
            roles = await _userManager.GetRolesAsync(user);
            await _userCacheManager.SetRolesAsync(request.UserId, roles);
        }

        return roles;
    }
}