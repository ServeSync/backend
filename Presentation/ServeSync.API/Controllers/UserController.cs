using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Application.Common;
using ServeSync.Application.Common.Dtos;
using ServeSync.Infrastructure.EfCore.Migrations;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Users.Queries;
using ServeSync.Infrastructure.Identity.UseCases.Users.Commands;
using ServeSync.Infrastructure.Identity.UseCases.Users.Queries;

namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}/permissions")]
    [HasPermission(AppPermissions.Users.ViewPermissions)]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPermissionsForUserAsync(string id, [FromQuery] Guid tenantId)
    {
        var permissions = await _mediator.Send(new GetAllPermissionForUserQuery(id, tenantId));
        return Ok(permissions);
    }
    
    [HttpGet("{id}/tenants/{tenantId}/roles")]
    [HasPermission(AppPermissions.Users.ViewRoles)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRolesForUserAsync(string id, Guid tenantId)
    {
        var roles = await _mediator.Send(new GetRolesForUserQuery(id, tenantId));
        return Ok(roles);
    }
    
    [HttpPut("{id}/tenants/{tenantId}/roles")]
    [HasPermission(AppPermissions.Users.EditRoles)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateRolesForUserAsync(string id, Guid tenantId, [FromBody] IEnumerable<string> roleIds)
    {
        await _mediator.Send(new UpdateRolesForUserCommand(id, tenantId, roleIds));
        return NoContent();
    }
    
    [HttpGet]
    [HasPermission(AppPermissions.Users.Management)]
    [ProducesResponseType(typeof(PagedResultDto<UserBasicInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserRequestDto dto)
    {
        var query = new GetAllUsersQuery(dto.Search, dto.Page, dto.Size, dto.Sorting);
        
        var users = await _mediator.Send(query);
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    [HasPermission(AppPermissions.Users.View)]
    [ProducesResponseType(typeof(UserDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByIdAsync(string id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }
    
}