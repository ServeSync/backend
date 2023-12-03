using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class UpdateSpecialProofCommand : ICommand
{
    public Guid Id { get; set; }
    public SpecialProofUpdateDto Proof { get; set; }
    
    public UpdateSpecialProofCommand(Guid id, SpecialProofUpdateDto proof)
    {
        Id = id;
        Proof = proof;
    }
}