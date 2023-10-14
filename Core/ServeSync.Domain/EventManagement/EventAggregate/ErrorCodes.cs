namespace ServeSync.Domain.EventManagement.EventAggregate;

public static class ErrorCodes
{
    public const string EventHeldShorter = "Event:000001";
    public const string NotInEventRegistrationTime = "Event:000002";
    
    public const string EventAttendanceInfoOverlapped = "EventAttendanceInfo:000001";
    public const string EventAttendanceInfoHeldShorter = "EventAttendanceInfo:000002";
    public const string EventAttendanceInfoOutOfEventTime = "EventAttendanceInfo:000003";
    
    public const string EventRoleHasAlreadyExist = "EventRole:000001";
    public const string EventRoleNotFound = "EventRole:000002";
    public const string EventRoleIsFullRegistered = "EventRole:000003";
    
    public const string OrganizationHasAlreadyAddedToEvent = "OrganizationInEvent:000001";
    public const string OrganizationHasNotAddedToEvent = "OrganizationInEvent:000002";
    
    public const string RepresentativeHasAlreadyAddedToEvent = "OrganizationRepInEvent:000001";

    public const string EventRegistrationHeldShorter = "EventRegistration:000001";
    public const string EventRegistrationOverlappedWithEvent = "EventRegistration:000002";
    public const string EventRegistrationInfoCannotBeUpdated = "EventRegistration:000003";
    public const string EventRegistrationInfoOverlapped = "EventRegistration:000004";

    public const string EventHeldShorter = "Event:001";
    public const string EventAttendanceInfoOverlapped = "EventAttendanceInfo:001";
    public const string EventAttendanceInfoHeldShorter = "EventAttendanceInfo:002";
    public const string EventAttendanceInfoOutOfEventTime = "EventAttendanceInfo:003";
    public const string EventRoleHasAlreadyExist = "EventRole:001";
    public const string OrganizationHasAlreadyAddedToEvent = "OrganizationInEvent:001";
    public const string OrganizationHasNotAddedToEvent = "OrganizationInEvent:002";
    public const string RepresentativeHasAlreadyAddedToEvent = "OrganizationRepInEvent:001";
}