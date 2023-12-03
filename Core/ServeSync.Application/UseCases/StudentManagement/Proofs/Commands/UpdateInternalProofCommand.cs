using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class UpdateInternalProofCommand : ICommand
{
    public Guid Id { get; set; }
    public InternalProofUpdateDto Proof { get; set; } 
    
    public UpdateInternalProofCommand(Guid id, InternalProofUpdateDto proof)
    {
        Id = id;
        Proof = proof;
    }
}