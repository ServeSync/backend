using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Dtos.Shared;
using ServeSync.Application.UseCases.EventManagement.Events.Commands;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos;

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
    
    [HttpPost]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto dto)
    {
        var id = await _mediator.Send(new CreateEventCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }
}