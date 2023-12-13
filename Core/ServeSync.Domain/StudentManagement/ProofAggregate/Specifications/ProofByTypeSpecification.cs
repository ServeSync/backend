using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

public class ProofByTypeSpecification : Specification<Proof, Guid>
{
    private readonly ProofType _type;
    
    public ProofByTypeSpecification(ProofType type)
    {
        _type = type;
    }
    
    public override Expression<Func<Proof, bool>> ToExpression()
    {
        return x => x.ProofType == _type;
    }
}
