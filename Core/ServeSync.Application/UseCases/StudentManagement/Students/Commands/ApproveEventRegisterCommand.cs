using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class ApproveEventRegisterCommand : ICommand
{
    public Guid StudentId { get; set; }
    public Guid EventRegisterId { get; set; }
    
    public ApproveEventRegisterCommand(Guid studentId, Guid eventRegisterId)
    {
        StudentId = studentId;
        EventRegisterId = eventRegisterId;
    }
}