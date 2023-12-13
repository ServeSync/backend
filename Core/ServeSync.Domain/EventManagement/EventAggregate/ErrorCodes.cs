namespace ServeSync.Domain.EventManagement.EventAggregate;

public static class ErrorCodes
{
    public const string EventHeldShorter = "Event:000001";
    public const string NotInEventRegistrationTime = "Event:000002";
    public const string EventNotFound = "Event:000003";
    public const string NotInEventAttendanceTime = "Event:000004";
    public const string EventCanNotBeCancelled = "Event:000005";
    public const string EventHasAlreadyStarted = "Event:000006";
    public const string EventCanNotBeApproved = "Event:000007";
    public const string EventCanNotBeRejected = "Event:000008";
    public const string EventCanNotBeUpdated = "Event:000009";
    public const string EventIsNotDone = "Event:000010";
    
    public const string EventAttendanceInfoOverlapped = "EventAttendanceInfo:000001";
    public const string EventAttendanceInfoHeldShorter = "EventAttendanceInfo:000002";
    public const string EventAttendanceInfoOutOfEventTime = "EventAttendanceInfo:000003";
    public const string EventAttendanceInfoNotFound = "EventAttendanceInfo:000004";
    public const string InvalidEventAttendanceCode = "EventAttendanceInfo:000005";
    public const string EventAttendanceInfoCanNotBeUpdated = "EventAttendanceInfo:000006";
    
    public const string EventRoleHasAlreadyExist = "EventRole:000001";
    public const string EventRoleNotFound = "EventRole:000002";
    public const string EventRoleIsFullRegistered = "EventRole:000003";
    public const string EventRoleCanNotBeUpdated = "EventRole:000004";
    
    public const string OrganizationHasAlreadyAddedToEvent = "OrganizationInEvent:000001";
    public const string OrganizationHasNotAddedToEvent = "OrganizationInEvent:000002";
    public const string EventOrganizationCanNotBeUpdated = "OrganizationInEvent:000003";
    
    public const string RepresentativeHasAlreadyAddedToEvent = "OrganizationRepInEvent:000001";
    public const string RepresentativeNotFoundInEvent = "OrganizationRepInEvent:000002";

    public const string EventRegistrationHeldShorter = "EventRegistration:000001";
    public const string EventRegistrationOverlappedWithEvent = "EventRegistration:000002";
    public const string EventRegistrationInfoCannotBeUpdated = "EventRegistration:000003";
    public const string EventRegistrationInfoOverlapped = "EventRegistration:000004";
    public const string EventRegistrationInfoNotFound = "EventRegistration:000005";
    public const string EventRegistrationInfoCannotBeAdded = "EventRegistration:000006";
}