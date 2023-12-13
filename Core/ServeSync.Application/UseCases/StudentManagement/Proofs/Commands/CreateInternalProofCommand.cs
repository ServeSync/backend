using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class CreateInternalProofCommand : ICommand<Guid>
{
    public InternalProofCreateDto Proof { get; set; }
    
    public CreateInternalProofCommand(InternalProofCreateDto proof)
    {
        Proof = proof;
    }
}