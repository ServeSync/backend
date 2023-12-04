using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

public class ProofByStudentIdSpecification : Specification<Proof, Guid>
{
    private readonly Guid _studentId;

    public ProofByStudentIdSpecification(Guid studentId, bool include = false)
    {
        _studentId = studentId;
        
        if (include)
        {
            AddInclude(x => x.Student!);
            AddInclude(x => x.ExternalProof!);
            AddInclude(x => x.ExternalProof!.Activity!);
            AddInclude(x => x.InternalProof!);
            AddInclude(x => x.InternalProof!.EventRole!);
            AddInclude(x => x.InternalProof!.Event!);
            AddInclude(x => x.InternalProof!.Event!.Activity!);
            AddInclude(x => x.InternalProof!.Event!.RepresentativeOrganization!.Organization!);
            AddInclude(x => x.SpecialProof!);
            AddInclude(x => x.SpecialProof!.Activity!);
        }
    }
    
    public override Expression<Func<Proof, bool>> ToExpression()
    {
        return x => x.StudentId == _studentId;
    }
}
