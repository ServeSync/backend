using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

public class InvalidEventAttendanceCodeException : ResourceInvalidDataException
{
    public InvalidEventAttendanceCodeException(string code) 
        : base($"Attendance code '{code}' is invalid or incorrect!", ErrorCodes.InvalidEventAttendanceCode)
    {
    }
}