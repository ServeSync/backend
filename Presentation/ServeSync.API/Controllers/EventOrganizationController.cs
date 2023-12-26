using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.Application.Common;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/event-organizations")]
public class EventOrganizationController : Controller
{
    private readonly IMediator _mediator;
    private ILogger<EventOrganizationController> _logger;
    
    public EventOrganizationController(IMediator mediator, ILogger<EventOrganizationController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [HasPermission(AppPermissions.EventOrganizations.View)]
    [ProducesResponseType(typeof(PagedResultDto<EventOrganizationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventOrganizationsAsync([FromQuery] EventOrganizationFilterRequestDto dto)
    {
        var query = new GetAllEventOrganizationQuery(dto.Status, dto.Search, dto.Page, dto.Size, dto.Sorting);
        var organizations = await _mediator.Send(query);
        return Ok(organizations);
    }
    
    [HttpGet("{id:guid}")]
    [HasPermission(AppPermissions.EventOrganizations.View)]
    [ProducesResponseType(typeof(EventOrganizationDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventOrganizationByIdAsync(Guid id)
    {
        var organization = await _mediator.Send(new GetEventOrganizationByIdQuery(id));
        return Ok(organization);
    }
    
    [HttpPost]
    [HasPermission(AppPermissions.EventOrganizations.Create)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEventOrganizationAsync([FromBody] EventOrganizationCreateDto dto)
    {
        var id = await _mediator.Send(new CreateEventOrganizationCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }
    
    [HttpPut("{id:guid}")]
    [HasPermission(AppPermissions.EventOrganizations.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEventOrganizationAsync(Guid id, [FromBody] EventOrganizationUpdateDto dto)
    {
        var command = new UpdateEventOrganizationCommand(id, dto.Name, dto.Description, dto.PhoneNumber, dto.Address, dto.ImageUrl);

        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [HasPermission(AppPermissions.EventOrganizations.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEventOrganizationByIdAsync(Guid id)
    {
        await _mediator.Send(new DeleteEventOrganizationCommand(id));
        return NoContent();
    }

    [HttpGet("{id:guid}/contacts")]
    [HasPermission(AppPermissions.EventOrganizations.ViewContact)]
    [ProducesResponseType(typeof(PagedResultDto<EventOrganizationContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContactByOrganizationAsync(Guid id, [FromQuery] EventOrganizationContactFilterRequestDto dto)
    {
        var contacts = await _mediator.Send(new GetAllEventOrganizationContactQuery(id, dto.Status, dto.Search, dto.Page, dto.Size, dto.Sorting));
        return Ok(contacts);
    }

    [HttpPost("{id:guid}/contacts")]
    [HasPermission(AppPermissions.EventOrganizations.AddContact)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateEventOrganizationContactAsync(Guid id, EventOrganizationContactCreateDto dto)
    {
        var contactId = await _mediator.Send(new CreateEventOrganizationContactCommand(id, dto));
        return Ok(SimpleIdResponse<Guid>.Create(contactId));
    }

    [HttpPut("{id:guid}/contacts/{contactId:guid}")]
    [HasPermission(AppPermissions.EventOrganizations.UpdateContact)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEventOrganizationContactByIdAsync(Guid id, Guid contactId,
        [FromBody] EventOrganizationContactUpdateDto dto)
    {
        await _mediator.Send(new UpdateEventOrganizationContactCommand(id, contactId, dto.Name, dto.Gender, dto.Birth, dto.PhoneNumber, dto.Address, dto.ImageUrl, dto.Position));
        return NoContent();
    }
    
    [HttpDelete("{id:guid}/contacts/{contactId:guid}")]
    [HasPermission(AppPermissions.EventOrganizations.RemoveContact)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEventOrganizationContactByIdAsync(Guid id, Guid contactId)
    {
        await _mediator.Send(new DeleteEventOrganizationContactCommand(id, contactId));
        return NoContent();
    }
    
    [HttpGet("invitations/approve")]
    public async Task<ActionResult> ApproveEventOrganizationInvitationAsync([FromQuery] string code)
    {
        try
        {
            await _mediator.Send(new ApproveEventOrganizationInvitationCommand(code));
            return View("ApproveEventOrganizationInvitation");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return View("ProcessEventOrganizationInvitationFailed");
        }
    }
    
    [HttpGet("invitations/reject")]
    public async Task<ActionResult> RejectEventOrganizationInvitationAsync([FromQuery] string code)
    {
        try
        {
            await _mediator.Send(new RejectEventOrganizationInvitationCommand(code));
            return View("RejectEventOrganizationInvitation");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return View("ProcessEventOrganizationInvitationFailed");
        }
    }
}