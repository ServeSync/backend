using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Controllers;

[Route("api/faculties")]
[ApiController]
public class FacultyController : ControllerBase
{
    private readonly IMediator _mediator;

    public FacultyController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    // [HasPermission(Permissions.Faculties.View)]
    [ProducesResponseType(typeof(IEnumerable<EducationProgramDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFacultiesAsync()
    {
        var faculties = await _mediator.Send(new GetAllFacultyQuery());
        return Ok(faculties);
    }
}