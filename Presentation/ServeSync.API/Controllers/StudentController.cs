using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Commands;
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

    [HttpPost]
    [HasPermission(Permissions.Students.Create)]
    [ProducesResponseType(typeof(SimpleIdResponse<Guid>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateStudentAsync(StudentCreateDto dto)
    {
        var command = new CreateStudentCommand(
            dto.Code, dto.FullName, dto.Gender, dto.Birth, dto.Email, dto.Phone,
            dto.Address, dto.ImageUrl, dto.HomeTown, dto.CitizenId, dto.HomeRoomId, dto.EducationProgramId);
        
        var studentId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStudentByIdAsync), new { id = studentId }, SimpleIdResponse<Guid>.Create(studentId));
    }
    
    [HttpPost("import")]
    [HasPermission(Permissions.Students.Create)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ImportStudentsAsync([FromForm] IFormFile file)
    {
        await _mediator.Send(new ImportStudentFromCsvCommand(file));
        return NoContent();
    }


    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetStudentByIdAsync))]
    [HasPermission(Permissions.Students.View)]
    [ProducesResponseType(typeof(FlatStudentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentByIdAsync(Guid id)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(id));
        return Ok(student);
    }
    
    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.Students.Edit)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStudentByIdAsync(Guid id, StudentUpdateDto dto)
    {
        var command = new UpdateStudentCommand(
            id, dto.Code, dto.FullName, dto.Gender, dto.Birth, dto.Email, dto.Phone,
            dto.Address, dto.ImageUrl, dto.CitizenId, dto.HomeTown, dto.HomeRoomId, dto.EducationProgramId);
        
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Students.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteStudentByIdAsync(Guid id)
    {
        await _mediator.Send(new DeleteStudentCommand(id));
        return Ok();
    }
    
    [HttpPost("{id:guid}/event-registers/{eventRegisterId:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveStudentEventRegisterAsync(Guid id, Guid eventRegisterId)
    {
        await _mediator.Send(new ApproveEventRegisterCommand(id, eventRegisterId));
        return NoContent();
    }
}