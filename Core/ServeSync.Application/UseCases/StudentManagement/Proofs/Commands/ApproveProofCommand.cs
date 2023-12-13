using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class ApproveProofCommand : ICommand
{
    public Guid Id { get; set; }
    
    public ApproveProofCommand(Guid id)
    {
        Id = id;
    }
}