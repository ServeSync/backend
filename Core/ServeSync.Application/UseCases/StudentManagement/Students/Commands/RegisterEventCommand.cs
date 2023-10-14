using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class RegisterEventCommand : ICommand
{
    public Guid EventRoleId { get; private set; }
    public string? Description { get; private set; }
    
    public RegisterEventCommand(
        Guid eventRoleId,
        string? description)
    {
        EventRoleId = eventRoleId;
        Description = description;
    }
}