using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class RejectEventRegisterCommand : ICommand
{
    public Guid StudentId { get; set; }
    public Guid EventRegisterId { get; set; }
    public string RejectReason { get; set; }
    
    public RejectEventRegisterCommand(Guid studentId, Guid eventRegisterId, string rejectReason)
    {
        StudentId = studentId;
        EventRegisterId = eventRegisterId;
        RejectReason = rejectReason;
    }
}