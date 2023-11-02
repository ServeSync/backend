namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate;

public static class ErrorCodes
{
    public const string EventOrganizationNotFound = "EventOrganization:000001";
    public const string EventOrganizationContactNotFound = "EventOrganizationContact:000002";
    public const string EventOrganizationContactDoesNotBelongToOrganization = "EventOrganizationContact:000003";
    public const string EventOrganizationContactAlreadyExisted = "EventOrganizationContact:000004";
}