using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Application.Common.Dtos;
using ServeSync.Infrastructure.EfCore.Migrations;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;
using ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;
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
    [HasPermission(Permissions.Users.ViewPermissions)]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPermissionsForUserAsync(string id, [FromQuery] Guid tenantId)
    {
        var permissions = await _mediator.Send(new GetAllPermissionForUserQuery(id, tenantId));
        return Ok(permissions);
    }
    
    [HttpGet]
    // [HasPermission(Permissions.Users.View)]
    [ProducesResponseType(typeof(PagedResultDto<UserBasicInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserRequestDto dto)
    {
        var query = new GetAllUsersQuery(dto.Search, dto.Page, dto.Size, dto.Sorting);
        
        var users = await _mediator.Send(query);
        return Ok(users);
    }
    
}