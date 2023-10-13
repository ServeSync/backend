namespace ServeSync.Domain.EventManagement.EventOrganizationAggregate;

public static class ErrorCodes
{
    public const string EventOrganizationNotFound = "EventOrganization:001";
    public const string EventOrganizationContactNotFound = "EventOrganizationContact:001";
    public const string EventOrganizationContactDoesNotBelongToOrganization = "EventOrganizationContact:002";
}