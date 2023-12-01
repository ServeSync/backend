using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.Application.Common;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;

namespace ServeSync.API.Controllers;

[Route("api/proofs")]
[ApiController]
public class ProofController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProofController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("internal")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateInternalProofAsync(InternalProofCreateDto dto)
    {
        var id = await _mediator.Send(new CreateInternalProofCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }

    [HttpPost("external")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateExternalProofAsync(ExternalProofCreateDto dto)
    {
        var id = await _mediator.Send(new CreateExternalProofCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ProofDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProofsAsync([FromQuery] ProofFilterRequestDto dto)
    {
        var query = new GetAllProofsQuery(dto.Search, dto.Status, dto.Type, dto.Page, dto.Size, dto.Sorting);
        
        var proofs = await _mediator.Send(query);
        return Ok(proofs);
    }
    
    [HttpPut("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RejectProofAsync([FromRoute] Guid id, [FromBody] RejectProofDto dto)
    {
        await _mediator.Send(new RejectProofCommand(id, dto.RejectReason));
        return NoContent();
    }
}