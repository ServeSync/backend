using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

public class InternalProof : Proof
{
    public Guid EventId { get; private set; }
    public Event? Event { get; set; }
    
    public Guid EventRoleId { get; private set; }
    public EventRole? EventRole { get; set; }
    
    public Proof? Proof { get; private set; }
    
    public InternalProof(
        string? description, 
        string imageUrl, 
        DateTime attendanceAt, 
        Guid studentId,
        Guid eventId,
        Guid eventRoleId) : base(ProofType.Internal, description, imageUrl, attendanceAt, studentId)
    {
        EventId = eventId;
        EventRoleId = eventRoleId;
    }
    
    public InternalProof()
    {
        
    }
}