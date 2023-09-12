using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Dtos.Roles;
using ServeSync.Application.Common.Dtos;
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
    [ProducesResponseType(typeof(PagedResultDto<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagedRoleAsync([FromQuery] RoleFilterAndPagingRequestDto dto)
    {
        var roles = await _mediator.Send(new GetAllRoleQuery(dto.Page, dto.Size, dto.Sorting, dto.Name));
        return Ok(roles);
    }
    
    [HttpGet("{id}")]
    [ActionName(nameof(GetRoleByIdAsync))]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleByIdAsync(string id)
    {
        var role = await _mediator.Send(new GetRoleByIdQuery(id));
        return Ok(role);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRoleAsync(CreateRoleDto dto)
    {
        var role = await _mediator.Send(new CreateRoleCommand(dto.Name));
        return CreatedAtAction(nameof(GetRoleByIdAsync), new { id = role.Id }, role);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRoleAsync(string id, UpdateRoleDto dto)
    {
        var role = await _mediator.Send(new UpdateRoleCommand(id, dto.Name));
        return Ok(role);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRoleAsync(string id)
    {
        await _mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }
}