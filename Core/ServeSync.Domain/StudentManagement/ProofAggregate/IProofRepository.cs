using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.ProofAggregate;

public interface IProofRepository : IRepository<Proof>
{
    Task<bool> IsInternalProofExistAsync(Guid eventId, Guid studentId);
    
}