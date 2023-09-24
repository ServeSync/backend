using ServeSync.Domain.StudentManagement.FacultyAggregate;
using ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class FacultyRepository : EfCoreRepository<Faculty>, IFacultyRepository
{
    public FacultyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}