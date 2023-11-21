using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ServeSync.Application.Identity;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Tenants.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

public class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, UserInfoDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly ITenantRepository _tenantRepository;
    private readonly IMapper _mapper;

    public GetUserInfoQueryHandler(
        ICurrentUser currentUser, 
        IUserRepository userRepository,
        IIdentityService identityService,
        ITenantRepository tenantRepository,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _identityService = identityService;
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }
    
    public async Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(_currentUser.Id);
        if (user == null)
        {
            throw new UserNotFoundException(_currentUser.Id);
        }

        var roles = await _identityService.GetRolesAsync(user.Id);
        var permissions = await _identityService.GetPermissionsForUserAsync(user.Id);
        var tenants = await _tenantRepository.FindByUserAsync(user.Id);

        var userInfo = new UserInfoDto()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Roles = roles,
            Permissions = permissions,
            TenantId = _currentUser.TenantId,
            Tenants = _mapper.Map<IEnumerable<TenantDto>>(tenants)
        };
        
        if (_currentUser.TenantId.HasValue)
        {
            var userInTenant = tenants.FirstOrDefault(x => x.Id == _currentUser.TenantId);
            if (userInTenant == null)
            {
                throw new UserNotInTenantException(_currentUser.Id, _currentUser.TenantId!.Value);
            }

            userInfo.FullName = userInTenant.Name;
            userInfo.AvatarUrl = userInTenant.AvatarUrl;
        }

        return userInfo;
    }
}