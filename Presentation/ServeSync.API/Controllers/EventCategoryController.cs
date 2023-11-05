using MediatR;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EventCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventCategoriesAsync([FromQuery] EventCategoryFilterRequestDto dto)
    {
        var categories = await _mediator.Send(new GetAllEventCategoryQuery(dto.Type));
        return Ok(categories);
    }

    [HttpGet("{id:guid}/activities")]
    [ProducesResponseType(typeof(IEnumerable<EventActivityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivitiesByCategoryAsync(Guid id)
    {
        var activities = await _mediator.Send(new GetAllEventActivityQuery(id));
        return Ok(activities);
    }
}