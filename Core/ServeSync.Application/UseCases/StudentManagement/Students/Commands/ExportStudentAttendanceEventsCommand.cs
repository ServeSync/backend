using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ExportStudentAttendanceEventsCommand : ICommand<byte[]>
{
    public Guid StudentId { get; set; }
    
    public ExportStudentAttendanceEventsCommand(Guid studentId)
    {
        StudentId = studentId;
    }
}