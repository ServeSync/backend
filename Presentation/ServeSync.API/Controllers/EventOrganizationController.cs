using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Dtos.EventOrganizations;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;

namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/event-organizations")]
public class EventOrganizationController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EventOrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id:guid}/contacts")]
    [ProducesResponseType(typeof(PagedResultDto<EventOrganizationContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContactByOrganizationAsync(Guid id, [FromQuery] EventOrganizationContactFilterRequestDto dto)
    {
        var contacts = await _mediator.Send(new GetAllEventOrganizationContactQuery(id, dto.Search, dto.Page, dto.Size, dto.Sorting));
        return Ok(contacts);
    }
}