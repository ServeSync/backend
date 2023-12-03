using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class CreateSpecialProofCommand : ICommand<Guid>
{
    public SpecialProofCreateDto Proof { get; set; } 
    
    public CreateSpecialProofCommand(SpecialProofCreateDto proof)
    {
        Proof = proof;
    }
}