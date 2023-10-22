using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Controllers;

[Route("api/homerooms")]
[ApiController]
public class HomeRoomController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomeRoomController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(Permissions.HomeRooms.View)]
    [ProducesResponseType(typeof(IEnumerable<HomeRoomDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHomeRoomsAsync([FromQuery] HomeRoomFilterRequestDto dto)
    {
        var homeRooms = await _mediator.Send(new GetAllHomeRoomQuery(dto.FacultyId));
        return Ok(homeRooms);
    }
}