﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.Application.Common;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Application.UseCases.Statistics.Queries;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;

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
    
    [HttpPost("special")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSpecialProofAsync(SpecialProofCreateDto dto)
    {
        var id = await _mediator.Send(new CreateSpecialProofCommand(dto));
        return Ok(SimpleIdResponse<Guid>.Create(id));
    }
    
    [HttpPut("{id:guid}/internal")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateInternalProofAsync([FromRoute] Guid id, [FromBody] InternalProofUpdateDto dto)
    {
        await _mediator.Send(new UpdateInternalProofCommand(id, dto));
        return NoContent();
    }
    
    [HttpPut("{id:guid}/external")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateExternalProofAsync([FromRoute] Guid id, [FromBody] ExternalProofUpdateDto dto)
    {
        await _mediator.Send(new UpdateExternalProofCommand(id, dto));
        return NoContent();
    }
    
    [HttpPut("{id:guid}/special")]
    [HasRole(AppRole.Student)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSpecialProofAsync([FromRoute] Guid id, [FromBody] SpecialProofUpdateDto dto)
    {
        await _mediator.Send(new UpdateSpecialProofCommand(id, dto));
        return NoContent();
    }

    [HttpGet]
    [HasPermission(AppPermissions.Proofs.Management)]
    [ProducesResponseType(typeof(PagedResultDto<ProofDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProofsAsync([FromQuery] ProofFilterRequestDto dto)
    {
        var query = new GetAllProofsQuery(dto.Search, dto.Status, dto.Type, dto.Page, dto.Size, dto.Sorting);
        
        var proofs = await _mediator.Send(query);
        return Ok(proofs);
    }
    
    [HttpGet("{id:guid}")]
    [HasPermission(AppPermissions.Proofs.View)]
    [ProducesResponseType(typeof(ProofDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProofByIdAsync(Guid id)
    {
        var proof = await _mediator.Send(new GetProofByIdQuery(id));
        return Ok(proof);
    }

    [HttpPut("{id:guid}/approve")]
    [HasPermission(AppPermissions.Proofs.Approve)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveProofAsync([FromRoute] Guid id)
    {
        await _mediator.Send(new ApproveProofCommand(id));
        return NoContent();
    }
    
    [HttpGet("{studentId:guid}/student")]
    [HasPermission(AppPermissions.Students.View)]
    [ProducesResponseType(typeof(PagedResultDto<ProofDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProofsOfStudentAsync([FromRoute] Guid studentId, [FromQuery] ProofFilterRequestDto dto)
    {
        var query = new GetAllProofsOfStudentQuery(dto.Search, studentId, dto.Status, dto.Type, dto.Page, dto.Size, dto.Sorting);
        var proofs = await _mediator.Send(query);
        
        return Ok(proofs);
    }
    
    [HttpPut("{id:guid}/reject")]
    [HasPermission(AppPermissions.Proofs.Reject)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RejectProofAsync([FromRoute] Guid id, [FromBody] RejectProofDto dto)
    {
        await _mediator.Send(new RejectProofCommand(id, dto.RejectReason));
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [HasPermission(AppPermissions.Proofs.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProofAsync([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteProofCommand(id));
        return NoContent();
    }
    
    [HttpGet("statistics")]
    [HasPermission(AppPermissions.Proofs.Management)]
    [ProducesResponseType(typeof(ProofStatisticDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProofStatisticAsync([FromQuery] ProofStatisticRequestDto dto)
    {
        var query = new GetProofStatisticQuery(dto.Type, dto.StartAt, dto.EndAt);
        var statistic = await _mediator.Send(query);
        return Ok(statistic);
    }
}