using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

public class SpecialProof : Proof
{
    public string Title { get; private set; }
    public string Role { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public double Score { get; private set; }
    
    public Guid ActivityId { get; private set; }
    public EventActivity? Activity { get; private set; }
    
    public Proof? Proof { get; private set; }
    
    public SpecialProof(
        string? description,
        string imageUrl, 
        Guid studentId,
        string title,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId) : base(ProofType.Special, description, imageUrl, null, studentId)
    {
        Title = title;
        Role = role;
        StartAt = startAt;
        EndAt = Guard.Range(endAt, nameof(StartAt), StartAt);
        Score = score;
        ActivityId = activityId;
    }
    
    internal void Update(
        string title,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId)
    {
        Title = title;
        Role = role;
        StartAt = startAt;
        EndAt = Guard.Range(endAt, nameof(StartAt), StartAt);
        Score = score;
        ActivityId = activityId;
    }
    
    public SpecialProof()
    {
        
    }
}