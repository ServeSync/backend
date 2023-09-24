using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class HomeRoomRepository : EfCoreRepository<HomeRoom>, IHomeRoomRepository
{
    public HomeRoomRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}