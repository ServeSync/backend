using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.API.Authorization;
using ServeSync.API.Common.Dtos;
using ServeSync.API.Common.Enums;
using ServeSync.Application.Common;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Queries;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Queries;
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
    [HasPermission(AppPermissions.Students.Management)]
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
    [HasPermission(AppPermissions.Students.Create)]
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
    [HasPermission(AppPermissions.Students.Import)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ImportStudentsAsync([FromForm] IFormFile file)
    {
        await _mediator.Send(new ImportStudentFromCsvCommand(file));
        return NoContent();
    }


    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetStudentByIdAsync))]
    [HasPermission(AppPermissions.Students.View)]
    [ProducesResponseType(typeof(FlatStudentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentByIdAsync(Guid id)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(id));
        return Ok(student);
    }
    
    [HttpPut("{id:guid}")]
    [HasPermission(AppPermissions.Students.Edit)]
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
    [HasPermission(AppPermissions.Students.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteStudentByIdAsync(Guid id)
    {
        await _mediator.Send(new DeleteStudentCommand(id));
        return NoContent();
    }
    
    [HttpPost("{id:guid}/event-registers/{eventRegisterId:guid}/approve")]
    [HasPermission(AppPermissions.Events.ApproveRegistration)]
    [EventAccessControl(EventSourceAccessControl.EventRegister)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveStudentEventRegisterAsync(Guid id, Guid eventRegisterId)
    {
        await _mediator.Send(new ApproveEventRegisterCommand(id, eventRegisterId));
        return NoContent();
    }
    
    [HttpPost("{id:guid}/event-registers/{eventRegisterId:guid}/reject")]
    [HasPermission(AppPermissions.Events.RejectRegistration)]
    [EventAccessControl(EventSourceAccessControl.EventRegister)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RejectStudentEventRegisterAsync(Guid id, Guid eventRegisterId, [FromBody] RejectStudentEventRegistrationDto dto)
    {
        await _mediator.Send(new RejectEventRegisterCommand(id, eventRegisterId, dto.RejectReason));
        return NoContent();
    }

    [HttpGet("{id:guid}/attendance-events")]
    [HasPermission(AppPermissions.Students.View)]
    [ProducesResponseType(typeof(PagedResultDto<StudentAttendanceEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttendanceEventsOfStudentAsync(Guid id, [FromQuery] AttendanceEventFilterRequestDto dto)
    {
        var events = await _mediator.Send(new GetAllAttendanceEventsOfStudentQuery(id, dto.IsPaging, dto.Page, dto.Size));
        return Ok(events);
    }
    
    [HttpGet("{id:guid}/registered-events")]
    [HasPermission(AppPermissions.Students.View)]
    [ProducesResponseType(typeof(PagedResultDto<StudentRegisteredEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegisteredEventsOfStudentAsync(Guid id, [FromQuery] PagingRequestDto dto)
    {
        var events = await _mediator.Send(new GetAllRegisteredEventsOfStudentQuery(id, dto.Page, dto.Size));
        return Ok(events);
    }
    
    [HttpGet("{id:guid}/education-program")]
    [HasPermission(AppPermissions.Students.View)]
    [ProducesResponseType(typeof(StudentEducationProgramDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentEducationProgramAsync(Guid id)
    {
        var program = await _mediator.Send(new GetEducationProgramByStudentQuery(id));
        return Ok(program);
    }
    
    [HttpPost("{id:guid}/attendance-events/export")]
    [HasPermission(AppPermissions.Students.Export)]
    public async Task<IActionResult> ExportStudentAttendanceEventsAsync(Guid id, ExportStudentAttendanceEventsDto dto)
    {
        var byteArray = await _mediator.Send(new ExportStudentAttendanceEventsCommand(id, dto.FromDate, dto.ToDate));
        return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ket-qua-pvcđ.xlsx");
    }
}