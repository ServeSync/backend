using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

public class ProofNotFoundException : ResourceNotFoundException
{
    public ProofNotFoundException(Guid id) : base(nameof(Proof), id, ErrorCodes.ProofNotFound)
    {
    }
} 
