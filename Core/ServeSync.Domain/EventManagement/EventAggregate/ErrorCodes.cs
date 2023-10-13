namespace ServeSync.Domain.EventManagement.EventAggregate;

public static class ErrorCodes
{
    public const string EventHeldShorter = "Event:001";
    public const string EventAttendanceInfoOverlapped = "EventAttendanceInfo:001";
    public const string EventAttendanceInfoHeldShorter = "EventAttendanceInfo:002";
    public const string EventRoleHasAlreadyExist = "EventRole:001";
    public const string OrganizationHasAlreadyAddedToEvent = "OrganizationInEvent:001";
    public const string OrganizationHasNotAddedToEvent = "OrganizationInEvent:002";
    public const string RepresentativeHasAlreadyAddedToEvent = "OrganizationRepInEvent:001";
}