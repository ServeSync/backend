using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.Application.Common;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

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
}