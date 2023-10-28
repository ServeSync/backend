namespace ServeSync.Domain.StudentManagement.StudentAggregate;

public static class ErrorCodes
{
    public const string DuplicateStudentCode = "Student:000001";
    public const string DuplicateStudentCitizenIdentifier = "Student:000002";
    public const string StudentNotFound = "Student:000003";
    public const string DuplicateStudentEmail = "Student:000004";
    public const string DuplicateStudentIdentity = "Student:000005";
    public const string StudentIdentityNotFound = "Student:000006";
    public const string StudentRegisteredToEventRole = "Student:000007";
    public const string StudentHasAlreadyApprovedToEvent = "Student:000008";
    public const string StudentAlreadyAttendance = "Student:000009";
    public const string StudentNotApprovedToEvent = "Student:000010";
    public const string StudentNotInAttendEventArea = "Student:000011";
    public const string StudentEventRegisterNotPending = "Student:000012";
    public const string StudentEventRegisterNotFound = "Student:000013";
}