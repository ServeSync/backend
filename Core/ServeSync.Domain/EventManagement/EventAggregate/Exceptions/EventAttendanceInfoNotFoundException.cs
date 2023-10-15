using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class EventAttendanceInfoNotFoundException : ResourceNotFoundException
{
    public EventAttendanceInfoNotFoundException(Guid id) : base(nameof(EventAttendanceInfo), id, ErrorCodes.EventAttendanceInfoNotFound)
    {
    }
}