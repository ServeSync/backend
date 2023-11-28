﻿using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class ProofRepository : EfCoreRepository<Proof>, IProofRepository
{
    public ProofRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> IsInternalProofExistAsync(Guid eventId, Guid studentId)
    {
        return DbContext.Set<InternalProof>()
            .AnyAsync(x => x.EventId == eventId && 
                           x.StudentId == studentId && 
                           x.ProofStatus == ProofStatus.Approved);
    }
}