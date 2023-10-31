using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Common.Dtos;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Queries;

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
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<EventCollaborationRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventCollaborationRequestsAsync([FromQuery] EventCollaborationRequestFilterRequestDto dto)
    {
        var query = new GetAllEventCollaborationRequestsQuery(dto.StartDate, dto.EndDate, dto.Type, dto.Search, dto.Page, dto.Size, dto.Sorting);
        
        var eventCollaborationRequests = await _mediator.Send(query);
        return Ok(eventCollaborationRequests);
    }
}