using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class ProofFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
    
    public ProofStatus? Status { get; set; }
    
    public ProofType? Type { get; set; }

    [SortConstraint(Fields = $"{nameof(Proof.Student.FullName)}, {nameof(Proof.Created)}, {nameof(Proof.LastModified)}")]
    public override string? Sorting { get; set; } = string.Empty;
}