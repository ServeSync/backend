using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.API.Common.Enums;
using ServeSync.Application.Common;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Commands;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventAttendanceInfos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;
using ServeSync.Application.UseCases.EventManagement.Events.Queries;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Application.UseCases.Statistics.Queries;
using ServeSync.Application.UseCases.StudentManagement.Students.Commands;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
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
    // [HasPermission(Permissions.Events.View)]
    [ProducesResponseType(typeof(PagedResultDto<FlatEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventsAsync([FromQuery] EventFilterRequestDto dto)
    {
        var query = new GetAllEventsQuery(dto.IsPaging, dto.StartDate, dto.EndDate, dto.EventType, dto.EventStatus, dto.DefaultFilters, dto.Search, dto.Page, dto.Size, dto.Sorting);
        
        var events = await _mediator.Send(query);
        return Ok(events);
    }
    
    [HttpGet("{id:guid}")]
    // [HasPermission(Permissions.Events.View)]
    [ProducesResponseType(typeof(EventDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventByIdAsync([FromRoute] Guid id)
    {
        var @event = await _mediator.Send(new GetEventByIdQuery(id));
        return Ok(@event);
    }
    
    [HttpPost]
    [HasPermission(Permissions.Events.Create)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto dto)
    {
        var id = await _mediator.Send(new CreateEventCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }
    
    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.Events.Edit)]
    [EventAccessControl(EventSourceAccessControl.Event)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventUpdateDto dto)
    {
        await _mediator.Send(new UpdateEventCommand(id, dto));
        return NoContent();
    }
    
    [HttpPost("register")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegisterEventAsync([FromBody] EventRegisterDto dto)
    {
        await _mediator.Send(new RegisterEventCommand(dto.EventRoleId, dto.Description));
        return NoContent();
    }
    
    [HttpGet("{id:guid}/registered-students")]
    [ProducesResponseType(typeof(PagedResultDto<RegisteredStudentInEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegisteredStudentsAsync(Guid id, [FromQuery] RegisteredStudentFilterRequestDto dto)
    {
        var registeredStudents = await _mediator.Send(new GetAllRegisteredStudentsQuery(id, dto.Status, dto.Page, dto.Size));
        return Ok(registeredStudents);
    }
    
    [HttpGet("{id:guid}/attendance-students")]
    [ProducesResponseType(typeof(PagedResultDto<AttendanceStudentInEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttendanceStudentsAsync(Guid id, [FromQuery] AttendanceStudentFilterRequestDto dto)
    {
        var attendanceStudents = await _mediator.Send(new GetAllAttendanceStudentsQuery(id, dto.Page, dto.Size));
        return Ok(attendanceStudents);
    }

    [HttpGet("{id:guid}/roles")]
    // [HasPermission(Permissions.Events.View)]
    [ProducesResponseType(typeof(IEnumerable<EventRoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventRolesAsync([FromRoute] Guid id)
    {
        var eventRoles = await _mediator.Send(new GetAllEventRolesQuery(id));
        return Ok(eventRoles);
    }
    
    [HttpGet("{id:guid}/event-attendances")]
    // [HasPermission(Permissions.Events.View)]
    [ProducesResponseType(typeof(IEnumerable<EventAttendanceInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventAttendancesInfoAsync([FromRoute] Guid id)
    {
        var eventAttendancesInfo = await _mediator.Send(new GetAllEventAttendanceInfosQuery(id));
        return Ok(eventAttendancesInfo);
    }

    [HttpPost("{id:guid}/event-attendances")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AttendEventAsync([FromRoute] Guid id, [FromBody] StudentAttendEventDto dto)
    {
        await _mediator.Send(new AttendEventCommand(id, dto.Code, dto.Longitude, dto.Latitude));
        return NoContent();
    }

    [HttpPost("{id:guid}/cancel")]
    [HasPermission(Permissions.Events.Cancel)]
    [EventAccessControl(EventSourceAccessControl.Event)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelEventAsync(Guid id)
    {
        await _mediator.Send(new CancelEventCommand(id));
        return NoContent();
    }
    
    [HttpPost("{id:guid}/approve")]
    [HasPermission(Permissions.Events.Approve)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveEventAsync(Guid id)
    {
        await _mediator.Send(new ApproveEventCommand(id));
        return NoContent();
    }
    
    [HttpPost("{id:guid}/reject")]
    [HasPermission(Permissions.Events.Reject)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RejectEventAsync(Guid id)
    {
        await _mediator.Send(new RejectEventCommand(id));
        return NoContent();
    }
    
    [HttpPost("sync")]
    [HasRole(AppRole.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SyncEventsAsync([FromQuery] Guid[] ids)
    {
        await _mediator.Send(new SyncEventsCommand(ids));
        return NoContent();
    }
    
    [HttpGet("statistic")]
    [HasPermission(Permissions.Events.Management)]
    [ProducesResponseType(typeof(EventStatisticDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventStatisticAsync([FromQuery] EventStatisticRequestDto dto)
    {
        var result = await _mediator.Send(new GetEventStatisticQuery(dto.Type));
        return Ok(result);
    }
    
    [HttpGet("registered-students/statistic")]
    [HasPermission(Permissions.Events.Management)]
    [ProducesResponseType(typeof(List<EventStudentStatisticDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventRegisteredStudentStatisticAsync([FromQuery] EventStudentStatisticRequestDto dto)
    {
        var result = await _mediator.Send(new GetEventRegisteredStudentStatisticQuery(dto.Type, dto.NumberOfRecords));
        return Ok(result);
    }
    
    [HttpGet("attendance-students/statistic")]
    [HasPermission(Permissions.Events.Management)]
    [ProducesResponseType(typeof(List<EventStudentStatisticDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventAttendanceStudentStatisticAsync([FromQuery] EventStudentStatisticRequestDto dto)
    {
        var result = await _mediator.Send(new GetEventAttendanceStudentStatisticQuery(dto.Type, dto.NumberOfRecords));
        return Ok(result);
    }
}