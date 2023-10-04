using Microsoft.EntityFrameworkCore;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class HomeRoomRepository : EfCoreRepository<HomeRoom>, IHomeRoomRepository
{
    public HomeRoomRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<HomeRoom>> FindByFacultyAsync(Guid? facultyId)
    {
        return await GetQueryable(new FilterHomeRoomSpecification(facultyId)).OrderBy(x => x.Name).ToListAsync();
    }
}