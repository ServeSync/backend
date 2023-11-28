using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}