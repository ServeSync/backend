using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;

public class GetAllProofsOfStudentQueryHandler : IQueryHandler<GetAllProofsOfStudentQuery, PagedResultDto<ProofDto>>
{
    private readonly IBasicReadOnlyRepository<Proof, Guid> _proofRepository;
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IMapper _mapper;
    
    public GetAllProofsOfStudentQueryHandler(
        IProofRepository proofRepository, 
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IMapper mapper)
    {
        _proofRepository = proofRepository;
        _studentRepository = studentRepository;
        _mapper = mapper;
    }
    
    public async Task<PagedResultDto<ProofDto>> Handle(GetAllProofsOfStudentQuery request, CancellationToken cancellationToken)
    {
        if (!await _studentRepository.IsExistingAsync(request.StudentId))
        {
            throw new StudentNotFoundException(request.StudentId);
        }
        
        var specification = GetQuerySpecification(request);
        
        var proofs = await _proofRepository.GetPagedListAsync(specification, new ProofDto());
        var total = await _proofRepository.GetCountAsync(specification);
        
        return new PagedResultDto<ProofDto>(
            total,
            request.Size,
            proofs);
    }
    
    private IPagingAndSortingSpecification<Proof, Guid> GetQuerySpecification(GetAllProofsOfStudentQuery request)
    {
        var specification = new FilterProofSpecification(request.Page, request.Size, request.Sorting, request.Search)
            .AndIf(request.Status.HasValue, new ProofByStatusSpecification(request.Status.GetValueOrDefault()))
            .AndIf(request.Type.HasValue, new ProofByTypeSpecification(request.Type.GetValueOrDefault()))
            .And(new ProofByStudentIdSpecification(request.StudentId));
        
        return specification;
    }
}
