using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ExportStudentAttendanceEventsCommand : ICommand<byte[]>
{
    public Guid StudentId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    public ExportStudentAttendanceEventsCommand(Guid studentId, DateTime fromDate, DateTime toDate)
    {
        StudentId = studentId;
        FromDate = fromDate;
        ToDate = toDate;
    }
}