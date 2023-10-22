using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Application.Common.Dtos;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Commands;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Commands;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Roles.Queries;

namespace ServeSync.API.Controllers;

[Route("api/roles")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HasPermission(Permissions.Roles.View)]
    [ProducesResponseType(typeof(PagedResultDto<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagedRoleAsync([FromQuery] RoleFilterAndPagingRequestDto dto)
    {
        var roles = await _mediator.Send(new GetAllRoleQuery(dto.Page, dto.Size, dto.Sorting, dto.Name));
        return Ok(roles);
    }
    
    [HttpGet("{id}")]
    [HasPermission(Permissions.Roles.View)]
    [ActionName(nameof(GetRoleByIdAsync))]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleByIdAsync(string id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        return Ok(role);
    }
    
    [HttpPost]
    [HasPermission(Permissions.Roles.Create)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRoleAsync(RoleCreateDto dto)
    {
        var role = await _mediator.Send(new CreateRoleCommand(dto.Name));
        return CreatedAtAction(nameof(GetRoleByIdAsync), new { id = role.Id }, role);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.Roles.Edit)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRoleAsync(string id, RoleUpdateDto dto)
    {
        var role = await _mediator.Send(new UpdateRoleCommand(id, dto.Name));
        return Ok(role);
    }
    
    [HttpDelete("{id}")]
    [HasPermission(Permissions.Roles.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRoleAsync(string id)
    {
        await _mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }
    
    [HttpGet("{id}/permissions")]
    [HasPermission(Permissions.Roles.ViewPermission)]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPermissionForRoleAsync(string id, [FromQuery] PermissionFilterRequestDto dto)
    {
        var permissions = await _mediator.Send(new GetAllPermissionForRoleQuery(id, dto.Name));
        return Ok(permissions);
    }
    
    [HttpPut("{id}/permissions")]
    [HasPermission(Permissions.Roles.UpdatePermission)]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePermissionForRoleAsync(string id, IEnumerable<Guid> permissionIds)
    {
        var permissions = await _mediator.Send(new UpdatePermissionForRoleCommand(id, permissionIds));
        return Ok(permissions);
    }
}