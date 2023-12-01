using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class RejectProofCommand : ICommand
{
    public Guid Id { get; set; }
    
    public string RejectReason { get; set; }
    
    public RejectProofCommand(Guid id, string rejectReason)
    {
        Id = id;
        RejectReason = rejectReason;
    }
}
