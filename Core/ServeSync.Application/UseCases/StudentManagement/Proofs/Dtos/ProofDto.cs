using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class ProofDto
{
    public Guid Id { get; set; }
    public ProofStatus ProofStatus { get; set; }
    public ProofType ProofType { get; set; }
    public string EventName { get; set; } = null!;
    public string OrganizationName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public SimpleStudentDto Student { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}