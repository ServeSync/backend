using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

public class ExternalProof : Proof
{
    public string EventName { get; private set; }
    public string Address { get; private set; }
    public string OrganizationName { get; private set; }
    public string Role { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public double Score { get; private set; }
    
    public Guid ActivityId { get; private set; }
    public EventActivity? Activity { get; private set; }
    
    public Proof? Proof { get; private set; }
    
    public ExternalProof(
        ProofType proofType, 
        string description, 
        string organizationName,
        string imageUrl, 
        DateTime attendanceAt, 
        Guid studentId,
        string eventName,
        string address,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId) : base(proofType, description, imageUrl, attendanceAt, studentId)
    {
        EventName = eventName;
        OrganizationName = organizationName;
        Address = address;
        Role = role;
        StartAt = startAt;
        EndAt = endAt;
        Score = score;
        ActivityId = activityId;
    }
    
    public ExternalProof()
    {
        
    }
}