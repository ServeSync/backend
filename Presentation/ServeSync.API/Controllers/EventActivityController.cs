using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;

namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/event-activities")]
public class EventActivityController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EventActivityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EventActivityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActivitiesAsync([FromQuery] EventActivityFilterRequestDto dto)
    {
        var activities = await _mediator.Send(new GetAllEventActivityQuery(null, dto.Type));
        return Ok(activities);
    }
}