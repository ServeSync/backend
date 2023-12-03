using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

public class Proof : AuditableAggregateRoot
{
    public ProofType ProofType { get; private set; }
    public ProofStatus ProofStatus { get; private set; }
    public string? Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime? AttendanceAt { get; private set; }
    
    public string? RejectReason { get; private set; }
    public Guid StudentId { get; private set; }
    public Student? Student { get; private set; }
    
    public InternalProof? InternalProof { get; private set; }
    public ExternalProof? ExternalProof { get; private set; }
    public SpecialProof? SpecialProof { get; private set; }
    
    public Proof(ProofType proofType, string? description, string imageUrl, DateTime? attendanceAt, Guid studentId)
    {
        ProofType = proofType;
        Description = description;
        ImageUrl = imageUrl;
        AttendanceAt = attendanceAt;
        StudentId = studentId;
        
        ProofStatus = ProofStatus.Pending;
    }

    internal void Reject(string reason)
    {
        if (ProofStatus != ProofStatus.Pending)
        {
            throw new ProofNotPendingException(Id);
        }
        ProofStatus = ProofStatus.Rejected;
        RejectReason = reason;
        AddDomainEvent(new ProofRejectedDomainEvent(this, reason));
    }
    
    internal void Approve()
    {
        if (ProofStatus != ProofStatus.Pending)
        {
            throw new ProofNotPendingException(Id);
        }
        
        ProofStatus = ProofStatus.Approved;
        AddDomainEvent(new ProofApprovedDomainEvent(this));
    }
    
    internal void Update(string? description, string imageUrl, DateTime? attendanceAt)
    {
        if (ProofStatus != ProofStatus.Pending)
        {
            throw new ProofNotPendingException(Id);
        }
        
        Description = description;
        ImageUrl = imageUrl;
        AttendanceAt = attendanceAt;
    }
    
    internal void UpdateInternalProof(Guid eventId, Guid eventRoleId)
    {
        InternalProof!.Update(eventId, eventRoleId);
    }
    
    internal void UpdateExternalProof(
        string eventName,
        string organizationName,
        string address,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId)
    {
        ExternalProof!.Update(
            eventName,
            organizationName,
            address,
            role,
            startAt,
            endAt,
            score,
            activityId);
    }
    
    internal void UpdateSpecialProof(
        string title,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId)
    {
        SpecialProof!.Update(
            title,
            role,
            startAt,
            endAt,
            score,
            activityId);
    }

    public Proof()
    {
        
    }
}