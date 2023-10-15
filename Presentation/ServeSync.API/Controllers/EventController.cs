using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Dtos.Events;
using ServeSync.API.Dtos.Shared;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Commands;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Queries;
using ServeSync.Application.UseCases.StudentManagement.Students.Commands;
using ServeSync.Infrastructure.Identity.Commons.Constants;
namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EventController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<FlatEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventsAsync([FromQuery] EventFilterRequestDto dto)
    {
        var query = new GetAllEventsQuery(dto.StartDate, dto.EndDate, dto.EventType, dto.EventStatus, dto.Search, dto.Page, dto.Size, dto.Sorting);
        
        var events = await _mediator.Send(query);
        return Ok(events);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto dto)
    {
        var id = await _mediator.Send(new CreateEventCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }
    
    [HttpPost("register")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegisterEventAsync([FromBody] EventRegisterDto dto)
    {
        await _mediator.Send(new RegisterEventCommand(dto.EventRoleId, dto.Description));
        return NoContent();
    }

    [HttpGet("{id:guid}/roles")]
    [ProducesResponseType(typeof(IEnumerable<EventRoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventRolesAsync([FromRoute] Guid id)
    {
        var eventRoles = await _mediator.Send(new GetAllEventRolesQuery(id));
        return Ok(eventRoles);
    }
    
    [HttpGet("{id:guid}/event-attendances")]
    [ProducesResponseType(typeof(PagedResultDto<EventAttendanceInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventAttendancesInfoAsync([FromRoute] Guid id)
    {
        var eventAttendancesInfo = await _mediator.Send(new GetAllEventAttendanceInfosQuery(id));
        return Ok(eventAttendancesInfo);
    }
}