using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Application.UseCases.Statistics.Queries;

namespace ServeSync.API.Controllers;

[Route("api/statistics")]
public class StatisticController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public StatisticController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(StatisticDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatistic()
    {
        var result = await _mediator.Send(new GetStatisticQuery());
        return Ok(result);
    }
}