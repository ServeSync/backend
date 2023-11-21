using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;

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

    [HttpGet]
    [HasPermission(Permissions.EventOrganizations.View)]
    [ProducesResponseType(typeof(PagedResultDto<EventOrganizationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventOrganizationsAsync([FromQuery] EventOrganizationFilterRequestDto dto)
    {
        var query = new GetAllEventOrganizationQuery(dto.Search, dto.Page, dto.Size, dto.Sorting);
        var organizations = await _mediator.Send(query);
        return Ok(organizations);
    }
    
    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.EventOrganizations.View)]
    [ProducesResponseType(typeof(EventOrganizationDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventOrganizationByIdAsync(Guid id)
    {
        var organization = await _mediator.Send(new GetEventOrganizationByIdQuery(id));
        return Ok(organization);
    }
    
    [HttpPost]
    [HasPermission(Permissions.EventOrganizations.Create)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEventOrganizationAsync([FromBody] EventOrganizationCreateDto dto)
    {
        var id = await _mediator.Send(new CreateEventOrganizationCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }
    
    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.EventOrganizations.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEventOrganizationAsync(Guid id, [FromBody] EventOrganizationUpdateDto dto)
    {
        var command = new UpdateEventOrganizationCommand(id, dto.Name, dto.Description, dto.Email, dto.PhoneNumber, dto.Address, dto.ImageUrl);

        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.EventOrganizations.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEventOrganizationByIdAsync(Guid id)
    {
        await _mediator.Send(new DeleteEventOrganizationCommand(id));
        return NoContent();
    }

    [HttpGet("{id:guid}/contacts")]
    [HasPermission(Permissions.EventOrganizations.View)]
    [ProducesResponseType(typeof(PagedResultDto<EventOrganizationContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContactByOrganizationAsync(Guid id, [FromQuery] EventOrganizationContactFilterRequestDto dto)
    {
        var contacts = await _mediator.Send(new GetAllEventOrganizationContactQuery(id, dto.Search, dto.Page, dto.Size, dto.Sorting));
        return Ok(contacts);
    }

    [HttpPut("{id:guid}/contacts/{contactId:guid}")]
    [HasPermission(Permissions.EventOrganizations.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEventOrganizationContactByIdAsync(Guid id, Guid contactId,
        [FromBody] EventOrganizationContactUpdateDto dto)
    {
        await _mediator.Send(new UpdateEventOrganizationContactCommand(id, contactId, dto.Name, dto.Gender, dto.Birth, dto.PhoneNumber, dto.Address, dto.ImageUrl, dto.Position));
        return NoContent();
    }
    
    [HttpDelete("{id:guid}/contacts/{contactId:guid}")]
    [HasPermission(Permissions.EventOrganizations.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEventOrganizationContactByIdAsync(Guid id, Guid contactId)
    {
        await _mediator.Send(new DeleteEventOrganizationContactCommand(id, contactId));
        return NoContent();
    }
}