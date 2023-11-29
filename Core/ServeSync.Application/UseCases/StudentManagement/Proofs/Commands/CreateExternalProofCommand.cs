using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class CreateExternalProofCommand : ICommand<Guid>
{
    public ExternalProofCreateDto Proof { get; set; }
    
    public CreateExternalProofCommand(ExternalProofCreateDto proof)
    {
        Proof = proof;
    }
}