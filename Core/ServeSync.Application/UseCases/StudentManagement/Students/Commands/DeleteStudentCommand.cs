using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Commands;

public class DeleteStudentCommand : ICommand
{
    public Guid Id { get; set; }

    public DeleteStudentCommand(Guid id)
    {
        Id = id;
    }
}