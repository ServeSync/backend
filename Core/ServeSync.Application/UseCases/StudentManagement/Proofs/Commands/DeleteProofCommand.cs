using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class DeleteProofCommand : ICommand
{
    public Guid Id { get; set; }

    public DeleteProofCommand(Guid id)
    {
        Id = id;
    }
}