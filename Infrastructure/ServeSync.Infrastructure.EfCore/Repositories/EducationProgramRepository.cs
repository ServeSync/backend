using ServeSync.Domain.StudentManagement.EducationProgramAggregate;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class EducationProgramRepository : EfCoreRepository<EducationProgram>, IEducationProgramRepository
{
    public EducationProgramRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}