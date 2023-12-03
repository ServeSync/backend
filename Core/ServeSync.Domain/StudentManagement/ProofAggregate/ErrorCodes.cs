namespace ServeSync.Domain.StudentManagement.ProofAggregate;

public static class ErrorCodes
{
    public const string InternalProofAlreadyExist = "Proof:000001";
    public const string ProofNotFound = "Proof:000002";
    public const string ProofNotPending = "Proof:000003";
    public const string ProofCanNotBeDeleted = "Proof:000004";
}