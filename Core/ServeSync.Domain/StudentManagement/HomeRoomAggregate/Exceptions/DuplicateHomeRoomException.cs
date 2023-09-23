using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.HomeRoomAggregate.Exceptions;

public class DuplicateHomeRoomException : ResourceAlreadyExistException
{
    public DuplicateHomeRoomException(string name)
        : base("HomeRoom", nameof(HomeRoom.Name), name, ErrorCodes.DuplicateHomeRoom)
    {
        
    }
}