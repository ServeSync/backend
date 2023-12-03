using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;

public interface IProofDomainService
{
    Task<Proof> CreateInternalProofAsync(
        string? description, 
        string imageUrl, 
        DateTime attendanceAt, 
        Guid studentId,
        Guid eventId,
        Guid eventRoleId,
        DateTime dateTime);

    Task<Proof> CreateExternalProofAsync(
        string? description,
        string imageUrl,
        DateTime? attendanceAt,
        Guid studentId,
        string eventName,
        string organizationName,
        string address,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId);
    
    Task<Proof> CreateSpecialProofAsync(
        string? description,
        string imageUrl,
        Guid studentId,
        string title,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId);

    Proof RejectProof(Proof proof, string reason);
    
    void Delete(Proof proof);
}