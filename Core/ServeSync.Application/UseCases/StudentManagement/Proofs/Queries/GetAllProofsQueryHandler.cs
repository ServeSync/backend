using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;

public class GetAllProofsQueryHandler : IQueryHandler<GetAllProofsQuery, PagedResultDto<ProofDto>>
{
    private readonly IBasicReadOnlyRepository<Proof, Guid> _proofRepository;
    private readonly IMapper _mapper;
    
    public GetAllProofsQueryHandler(IBasicReadOnlyRepository<Proof, Guid> proofRepository, IMapper mapper)
    {
        _proofRepository = proofRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<ProofDto>> Handle(GetAllProofsQuery request, CancellationToken cancellationToken)
    {
        var specification = GetQuerySpecification(request);
        
        var proofs = await _proofRepository.GetPagedListAsync(specification, new ProofDto());
        var total = await _proofRepository.GetCountAsync(specification);
        
        return new PagedResultDto<ProofDto>(
            total,
            request.Size,
            proofs);
    }
    
    private IPagingAndSortingSpecification<Proof, Guid> GetQuerySpecification(GetAllProofsQuery request)
    {
        var specification = new FilterProofSpecification(request.Page, request.Size, request.Sorting, request.Search)
            .AndIf(request.ProofStatus.HasValue, new ProofByStatusSpecification(request.ProofStatus.GetValueOrDefault()))
            .AndIf(request.ProofType.HasValue, new ProofByTypeSpecification(request.ProofType.GetValueOrDefault()));
        
        return specification;
    }
}