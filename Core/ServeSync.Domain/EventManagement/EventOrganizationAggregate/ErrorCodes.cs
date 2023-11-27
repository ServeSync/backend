namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate;

public static class ErrorCodes
{
    public const string EventOrganizationNotFound = "EventOrganization:000001";
    public const string DuplicateEventOrganizationEmail = "EventOrganization:000002";
    public const string EventOrganizationHasHostedEvent = "EventOrganization:000003";
    public const string DuplicateEventOrganizationName = "EventOrganization:000004";
    public const string EventOrganizationNotPending = "EventOrganization:000005";
    public const string EventOrganizationNotActive = "EventOrganization:000006";
    
    public const string EventOrganizationContactNotFound = "EventOrganizationContact:000001";
    public const string EventOrganizationContactDoesNotBelongToOrganization = "EventOrganizationContact:000002";
    public const string EventOrganizationContactAlreadyExisted = "EventOrganizationContact:000003";
    public const string EventOrganizationContactHasAttendAnEvent = "EventOrganizationContact:000004";
    public const string EventOrganizationContactNotPending = "EventOrganizationContact:000005";
    public const string EventOrganizationContactNotActive = "EventOrganizationContact:000006";
    
    public const string OrganizationInvitationNotFound = "OrganizationInvitation:000001";
}