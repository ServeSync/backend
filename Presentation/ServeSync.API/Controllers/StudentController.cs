using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Dtos.Students;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Queries;
using ServeSync.Infrastructure.Identity.Commons.Constants;

namespace ServeSync.API.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HasPermission(Permissions.Students.View)]
    [ProducesResponseType(typeof(PagedResultDto<StudentDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStudentsAsync([FromQuery] StudentFilterRequestDto dto)
    {
        var query = new GetAllStudentQuery(
            dto.HomeRoomId, dto.FacultyId, dto.EducationProgramId, dto.Gender,
            dto.Search, dto.Page, dto.Size, dto.Sorting);
        
        var students = await _mediator.Send(query);
        return Ok(students);
    }


    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.Students.View)]
    [ProducesResponseType(typeof(FlatStudentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentByIdAsync(Guid id)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(id));
        return Ok(student);
    }
}