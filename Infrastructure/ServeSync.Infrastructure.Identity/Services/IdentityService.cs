using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ServeSync.Application.Common;
using ServeSync.Application.Identity;
using ServeSync.Application.Identity.Dtos;
using ServeSync.Domain.SeedWorks.Exceptions;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

namespace ServeSync.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantService _tenantService;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public IdentityService(
        IMediator mediator, 
        IMapper mapper,
        ITenantRepository tenantRepository,
        ITenantService tenantService,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _mapper = mapper;
        _tenantRepository = tenantRepository;
        _tenantService = tenantService;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<IdentityUserDto?> GetByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return _mapper.Map<IdentityUserDto>(user);
    }

    public async Task<IdentityUserDto?> GetByUserNameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return _mapper.Map<IdentityUserDto>(user);
    }

    public async Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId, Guid tenantId)
    {
        var permissions = await _mediator.Send(new GetAllPermissionForUserQuery(userId, tenantId));
        return permissions.Select(x => x.Name);
    }

    public async Task<IEnumerable<string>> GetRolesAsync(string userId, Guid tenantId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Array.Empty<string>();
        }

        return await _userRepository.GetRolesAsync(userId, tenantId);
    }

    public async Task<bool> HasPermissionAsync(string userId, Guid tenantId, string permission)
    {
        var permissions = await GetPermissionsForUserAsync(userId, tenantId);
        return permissions.Contains(permission);
    }

    public async Task<IdentityResult<IdentityUserDto>> CreateUserAsync(string fullname, string username, string avatarUrl, string email, string password, string? phone, Guid? externalId)
    {
        var user = new ApplicationUser(fullname, avatarUrl, externalId)
        {
            UserName = username,
            Email = email,
            PhoneNumber = phone
        };

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return IdentityResult<IdentityUserDto>.Success(_mapper.Map<IdentityUserDto>(user));
        }

        var error = result.Errors.First();
        return IdentityResult<IdentityUserDto>.Failed(error.Code, error.Description);
    }

    public async Task<IdentityResult<IdentityUserDto>> CreateStudentAsync(
        string fullname, 
        string username, 
        string avatarUrl, 
        string email, 
        string password, 
        Guid studentId, 
        string? phone = null)
    {
        var createIdentityUserResult = await CreateUserAsync(fullname, username, avatarUrl, email, password, phone, studentId);
        if (createIdentityUserResult.IsSuccess)
        {
            var tenant = await _tenantRepository.FindByIdAsync(AppTenant.Default);
            await _tenantService.AddUserToTenantAsync(createIdentityUserResult.Data!.Id, fullname, avatarUrl, false, tenant!.Id);
            var grantRoleResult = await GrantToRoleAsync(createIdentityUserResult.Data!.Id, AppRole.Student, tenant!.Id);
            if (grantRoleResult.IsSuccess)
            {
                return IdentityResult<IdentityUserDto>.Success(createIdentityUserResult.Data);
            }
            else
            {
                return IdentityResult<IdentityUserDto>.Failed(grantRoleResult.Error!, grantRoleResult.ErrorCode!);
            }
        }

        return createIdentityUserResult;
    }

    public async Task<IdentityResult<IdentityUserDto>> CreateEventOrganizationContactAsync(
        string fullname, 
        string username, 
        string avatarUrl, 
        string email, 
        string password, 
        Guid contactId, 
        Guid tenantId,
        string? phone = null)
    {
        var createIdentityUserResult = await CreateUserAsync(fullname, username, avatarUrl, email, password, phone, contactId);
        if (createIdentityUserResult.IsSuccess)
        {
            var tenant = await _tenantRepository.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                return IdentityResult<IdentityUserDto>.Failed(IdentityErrorCodes.IdentityTenantNotFound, $"Tenant with id {tenantId} not found!");
            }
            
            var grantRoleResult = await GrantToRoleAsync(createIdentityUserResult.Data!.Id, AppRole.EventOrganizer, tenantId);
            if (grantRoleResult.IsSuccess)
            {
                return IdentityResult<IdentityUserDto>.Success(createIdentityUserResult.Data);
            }
            else
            {
                return IdentityResult<IdentityUserDto>.Failed(grantRoleResult.Error!, grantRoleResult.ErrorCode!);
            }
        }

        return createIdentityUserResult;
    }

    public async Task<IdentityResult<IdentityUserDto>> CreateEventOrganizationAsync(
        string fullname, 
        string username, 
        string avatarUrl, 
        string email, 
        string password, 
        Guid organizationId, 
        Guid tenantId,
        string? phone = null)
    {
        var createIdentityUserResult = await CreateUserAsync(fullname, username, avatarUrl, email, password, phone, organizationId);
        if (createIdentityUserResult.IsSuccess)
        {
            var tenant = await _tenantRepository.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                return IdentityResult<IdentityUserDto>.Failed(IdentityErrorCodes.IdentityTenantNotFound, $"Tenant with id {tenantId} not found!");
            }
            
            var grantRoleResult = await GrantToRoleAsync(createIdentityUserResult.Data!.Id, AppRole.EventOrganization, tenantId);
            if (grantRoleResult.IsSuccess)
            {
                return IdentityResult<IdentityUserDto>.Success(createIdentityUserResult.Data);
            }
            else
            {
                return IdentityResult<IdentityUserDto>.Failed(grantRoleResult.Error!, grantRoleResult.ErrorCode!);
            }
        }

        return createIdentityUserResult;
    }

    public async Task<IdentityResult<bool>> UpdateAsync(string userId, string fullname, string email, string avatarUrl)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityUserNotFound, $"User with id {userId} not found!");
        }
        
        user.UpdateFullName(fullname);
        user.SetAvatar(avatarUrl);
        user.Email = email;
        
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return IdentityResult<bool>.Success(true);
        }
        
        var error = result.Errors.First();
        return IdentityResult<bool>.Failed(error.Code, error.Description);
    }

    public async Task<IdentityResult<bool>> UpdateUserNameAsync(string userId, string username)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityUserNotFound, $"User with id {userId} not found!");
        }
        
        user.UserName = username;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return IdentityResult<bool>.Success(true);
        }
        
        var error = result.Errors.First();
        return IdentityResult<bool>.Failed(error.Code, error.Description);
    }

    public async Task<IdentityResult<bool>> DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityUserNotFound, $"User with id {userId} not found!");
        }
        
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return IdentityResult<bool>.Success(true);
        }
        
        var error = result.Errors.First();
        return IdentityResult<bool>.Failed(error.Code, error.Description);
    }

    public async Task<bool> IsOrganizationContactAsync(string userId, Guid tenantId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var roles = await GetRolesAsync(userId, tenantId);
        return roles.Contains(AppRole.EventOrganizer);
    }

    public async Task<bool> IsEventOrganizationAsync(string userId, Guid tenantId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var roles = await GetRolesAsync(userId, tenantId);
        return roles.Contains(AppRole.EventOrganization);
    }

    public async Task<IdentityResult<bool>> GrantToRoleAsync(string userId, string roleName, Guid tenantId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityUserNotFound, $"User with id {userId} not found!");
        }

        try
        {
            var role = await _roleRepository.FindByNameAsync(roleName);
            if (role == null)
            {
                return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityRoleNotFound, $"Role with name {roleName} not found!");
            }
            
            user.GrantRole(role.Id, tenantId);
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return IdentityResult<bool>.Success(true);
            }
            
            var error = result.Errors.First();
            return IdentityResult<bool>.Failed(error.Code, error.Description);
        }
        catch (CoreException e)
        {
            return IdentityResult<bool>.Failed(e.ErrorCode, e.Message);
        }
    }

    public async Task<IdentityResult<bool>> UnGrantFromRoleAsync(string userId, string roleName, Guid tenantId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityUserNotFound, $"User with id {userId} not found!");
        }

        try
        {
            var role = await _roleRepository.FindByNameAsync(roleName);
            if (role == null)
            {
                return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityRoleNotFound, $"Role with name {roleName} not found!");
            }
            
            user.UnGrantRole(role.Id, tenantId);
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return IdentityResult<bool>.Success(true);
            }
            
            var error = result.Errors.First();
            return IdentityResult<bool>.Failed(error.Code, error.Description);
        }
        catch
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityRoleNotFound, $"Role with name {roleName} not found!");
        }
    }
}