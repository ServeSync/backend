using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.Exceptions;

public class HomeRoomNotFoundException : ResourceNotFoundException
{
    public HomeRoomNotFoundException(Guid id) 
        : base(nameof(HomeRoom), id, ErrorCodes.HomeRoomNotFound)
    {
    }
}