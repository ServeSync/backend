using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Queries;

public class GetAllProofsOfStudentQuery : PagingAndSortingRequestDto, IQuery<PagedResultDto<ProofDto>>
{
    public string? Search { get; set; }
    public Guid StudentId { get; set; }
    public ProofStatus? Status { get; set; }
    public ProofType? Type { get; set; }

    public GetAllProofsOfStudentQuery(
        string? search,
        Guid studentId, 
        ProofStatus? status,
        ProofType? type,
        int page, 
        int size,
        string sorting) : base(page, size, sorting)
    {
        StudentId = studentId;
        Status = status;
        Type = type;
    }
}