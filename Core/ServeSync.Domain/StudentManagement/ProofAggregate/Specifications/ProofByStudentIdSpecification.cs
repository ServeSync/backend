using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

public class ProofByStudentIdSpecification : Specification<Proof, Guid>
{
    private readonly Guid _studentId;

    public ProofByStudentIdSpecification(Guid studentId)
    {
        _studentId = studentId;
        
    }
    
    public override Expression<Func<Proof, bool>> ToExpression()
    {
        return x => x.StudentId == _studentId;
    }
}
