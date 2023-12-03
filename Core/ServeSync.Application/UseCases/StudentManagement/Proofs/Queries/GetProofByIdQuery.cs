using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;

public class GetProofByIdQuery : IQuery<ProofDetailDto>
{
    public Guid Id { get; set; }
    
    public GetProofByIdQuery(Guid id)
    {
        Id = id;
    }
}