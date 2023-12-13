using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

public class ProofByStatusSpecification : Specification<Proof, Guid>
{
    private readonly ProofStatus _status;
    
    public ProofByStatusSpecification(ProofStatus status)
    {
        _status = status;
    }
    
    public override Expression<Func<Proof, bool>> ToExpression()
    {
        return x => x.ProofStatus == _status;
    }
}
