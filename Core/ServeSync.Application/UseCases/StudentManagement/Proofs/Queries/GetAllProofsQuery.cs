using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;

public class GetAllProofsQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<ProofDto>>
{
    public string? Search { get; set; }
    public ProofStatus? ProofStatus { get; set; }
    public ProofType? ProofType { get; set; }
    
    public GetAllProofsQuery(
        string? search, 
        ProofStatus? proofStatus, 
        ProofType? proofType,
        int page, 
        int size, 
        string sorting) 
        : base(page, size, sorting)
    {
        Search = search;
        ProofStatus = proofStatus;
        ProofType = proofType;
    }
}