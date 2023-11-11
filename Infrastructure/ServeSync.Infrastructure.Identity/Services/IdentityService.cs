using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ServeSync.Application.Identity;
using ServeSync.Application.Identity.Dtos;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

namespace ServeSync.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public IdentityService(
        IMediator mediator, 
        IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IdentityUserDto?> GetByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return _mapper.Map<IdentityUserDto>(user);
    }

    public async Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId)
    {
        var permissions = await _mediator.Send(new GetAllPermissionForUserQuery(userId));
        return permissions.Select(x => x.Name);
    }

    public async Task<IEnumerable<string>> GetRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Array.Empty<string>();
        }

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission)
    {
        var permissions = await GetPermissionsForUserAsync(userId);
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

    public async Task<IdentityResult<IdentityUserDto>> CreateStudentAsync(string fullname, string username, string avatarUrl, string email, string password, Guid studentId, string? phone = null)
    {
        var createIdentityUserResult = await CreateUserAsync(fullname, username, avatarUrl, email, password, phone, studentId);
        if (createIdentityUserResult.IsSuccess)
        {
            var grantRoleResult = await GrantToRoleAsync(createIdentityUserResult.Data!.Id, AppRole.Student);
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

    public async Task<IdentityResult<IdentityUserDto>> CreateEventOrganizationContactAsync(string fullname, string username, string avatarUrl, string email, string password, Guid contactId, string? phone = null)
    {
        var createIdentityUserResult = await CreateUserAsync(fullname, username, avatarUrl, email, password, phone, contactId);
        if (createIdentityUserResult.IsSuccess)
        {
            var grantRoleResult = await GrantToRoleAsync(createIdentityUserResult.Data!.Id, AppRole.EventOrganizer);
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

    public async Task<IdentityResult<bool>> GrantToRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityUserNotFound, $"User with id {userId} not found!");
        }

        try
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return IdentityResult<bool>.Success(true);
            }
            
            var error = result.Errors.First();
            return IdentityResult<bool>.Failed(error.Code, error.Description);
        }
        catch
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityRoleNotFound, $"Role with name {role} not found!");
        }
    }

    public async Task<IdentityResult<bool>> UnGrantFromRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityUserNotFound, $"User with id {userId} not found!");
        }

        try
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                return IdentityResult<bool>.Success(true);
            }
            
            var error = result.Errors.First();
            return IdentityResult<bool>.Failed(error.Code, error.Description);
        }
        catch
        {
            return IdentityResult<bool>.Failed(IdentityErrorCodes.IdentityRoleNotFound, $"Role with name {role} not found!");
        }
    }
}