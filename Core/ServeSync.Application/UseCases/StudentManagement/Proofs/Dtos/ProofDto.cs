using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class ProofDto
{
    public Guid Id { get; set; }
    public ProofStatus ProofStatus { get; set; }
    public ProofType ProofType { get; set; }
    public string EventName { get; set; } 
    public string OrganizationName { get; set; }
    public string Address { get; set; }
    public string ImageUrl { get; set; } 
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public SimpleStudentDto Student { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}