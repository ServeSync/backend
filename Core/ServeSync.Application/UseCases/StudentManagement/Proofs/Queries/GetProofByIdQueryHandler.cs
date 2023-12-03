using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;

public class GetProofByIdQueryHandler : IQueryHandler<GetProofByIdQuery, ProofDetailDto>
{
    private readonly IBasicReadOnlyRepository<Proof, Guid> _proofRepository;
    
    public GetProofByIdQueryHandler(IBasicReadOnlyRepository<Proof, Guid> proofRepository)
    {
        _proofRepository = proofRepository;
    }
    
    public async Task<ProofDetailDto> Handle(GetProofByIdQuery request, CancellationToken cancellationToken)
    {
        var proof = await _proofRepository.FindByIdAsync<ProofDetailDto>(request.Id, new ProofDetailDto());
        if (proof == null)
        {
            throw new ProofNotFoundException(request.Id);
        }
        
        return proof;
    }
}