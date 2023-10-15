using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class AttendEventCommand : ICommand
{
    public Guid EventId { get; set; }
    public string Code { get; set; }
    
    public AttendEventCommand(Guid eventId, string code)
    {
        EventId = eventId;
        Code = code;
    }
}