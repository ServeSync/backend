using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class UpdateExternalProofCommand : ICommand
{
    public Guid Id { get; set; }
    public ExternalProofUpdateDto Proof { get; set; } 
    
    public UpdateExternalProofCommand(Guid id, ExternalProofUpdateDto proof)
    {
        Id = id;
        Proof = proof;
    }
}