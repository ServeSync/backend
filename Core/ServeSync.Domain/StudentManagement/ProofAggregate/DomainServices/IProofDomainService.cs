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
}