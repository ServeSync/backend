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
    [ProducesResponseType(typeof(PagedResultDto<EventCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventCategoriesAsync([FromQuery] EventCategoryFilterRequestDto dto)
    {
        var query = new GetAllEventCategoryQuery(dto.Search, dto.Page, dto.Size, dto.Sorting);
        var categories = await _mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{id:guid}/activities")]
    [ProducesResponseType(typeof(PagedResultDto<EventActivityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivitiesByCategoryAsync(Guid id, [FromQuery] EventActivityByCategoryFilterRequestDto dto)
    {
        var activities = await _mediator.Send(new GetAllEventActivityQuery(id, dto.Search, dto.Page, dto.Size, dto.Sorting));
        return Ok(activities);
    }
}