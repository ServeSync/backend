using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Dtos.Shared;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/event-collaboration-requests")]
public class EventCollaborationRequest : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EventCollaborationRequest(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEvent([FromBody] EventCollaborationRequestCreateDto dto)
    {
        var id = await _mediator.Send(new CreateEventCollaborationCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }
}