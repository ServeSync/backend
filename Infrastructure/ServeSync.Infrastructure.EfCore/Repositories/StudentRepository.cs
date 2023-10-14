using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class StudentRepository : EfCoreRepository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext dbContext) : base(dbContext)
    {
        AddInclude(x => x.EventRegisters);
    }

    public Task<Student?> FindByIdentityAsync(string identityId)
    {
        return GetQueryable().FirstOrDefaultAsync(x => x.IdentityId == identityId);
    }
}