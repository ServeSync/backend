using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

public class Proof : AuditableAggregateRoot
{
    public ProofType ProofType { get; private set; }
    public ProofStatus ProofStatus { get; private set; }
    public string? Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime? AttendanceAt { get; private set; }
    public Guid StudentId { get; private set; }
    public Student? Student { get; private set; }
    
    public InternalProof? InternalProof { get; private set; }
    public ExternalProof? ExternalProof { get; private set; }
    
    public Proof(ProofType proofType, string? description, string imageUrl, DateTime? attendanceAt, Guid studentId)
    {
        ProofType = proofType;
        Description = description;
        ImageUrl = imageUrl;
        AttendanceAt = attendanceAt;
        StudentId = studentId;
        
        ProofStatus = ProofStatus.Pending;
    }

    public Proof()
    {
        
    }
}