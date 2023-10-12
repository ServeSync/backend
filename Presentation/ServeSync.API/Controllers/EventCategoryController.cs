using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Dtos.EventCategories;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;

namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/event-categories")]
public class EventCategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EventCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}/activities")]
    [ProducesResponseType(typeof(PagedResultDto<EventActivityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivitiesByCategoryAsync(Guid id, [FromQuery] EventActivityByCategoryFilterRequestDto dto)
    {
        var activities = await _mediator.Send(new GetAllEventActivityQuery(id, dto.Search, dto.Page, dto.Size, dto.Sorting));
        return Ok(activities);
    }
}