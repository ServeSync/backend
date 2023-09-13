using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Dtos.Permissions;
using ServeSync.Application.Common.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

namespace ServeSync.API.Controllers;

[Route("api/permissions")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagedRoleAsync([FromQuery] PermissionFilterRequestDto dto)
    {
        var permissions = await _mediator.Send(new GetAllPermissionQuery(dto.Name));
        return Ok(permissions);
    }
}