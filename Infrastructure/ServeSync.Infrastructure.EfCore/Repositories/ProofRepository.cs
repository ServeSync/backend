using System.Collections.ObjectModel;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class ProofRepository : EfCoreRepository<Proof>, IProofRepository
{
    public ProofRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.InternalProof!);
        AddInclude(x => x.ExternalProof!);
        AddInclude(x => x.SpecialProof!);
    }

    public Task<bool> IsInternalProofExistAsync(Guid eventId, Guid studentId)
    {
        return DbContext.Set<InternalProof>()
            .AnyAsync(x => x.EventId == eventId && 
                           x.StudentId == studentId && 
                           x.ProofStatus == ProofStatus.Approved);
    }

    public Task<double> GetSumScoreOfStudentAsync(Guid studentId)
    {
        return DbSet.Where(x => x.StudentId == studentId && x.ProofStatus == ProofStatus.Approved)
            .SumAsync(x => x.ProofType == ProofType.Internal 
                ? x.InternalProof!.EventRole!.Score
                : x.ProofType == ProofType.External
                    ? x.ExternalProof!.Score
                    : x.SpecialProof!.Score);
    }

    public async Task<IList<Proof>> GetProofsByInternalEventAsync(Guid eventId, Guid studentId)
    {
        return await GetQueryable()
            .Where(x => x.StudentId == studentId &&
                        x.ProofStatus == ProofStatus.Pending &&
                        x.InternalProof!.EventId == eventId)
            .ToListAsync();
    }
}