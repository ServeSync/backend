using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Dtos.Events;
using ServeSync.API.Dtos.Shared;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Commands;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Queries;

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
}