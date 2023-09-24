using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Controllers;

[Route("api/education-programs")]
[ApiController]
public class EducationProgramController : ControllerBase
{
    private readonly IMediator _mediator;

    public EducationProgramController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(Permissions.EducationPrograms.View)]
    [ProducesResponseType(typeof(IEnumerable<EducationProgramDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEducationProgramsAsync()
    {
        var educationPrograms = await _mediator.Send(new GetAllEducationProgramQuery());
        return Ok(educationPrograms);
    }
}