using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

public class ProofByCreateTimeSpecification : Specification<Proof, Guid>
{
    private readonly DateTime? _fromDate;
    private readonly DateTime? _toDate;
    
    public ProofByCreateTimeSpecification(DateTime? fromDate, DateTime? toDate)
    {
        _fromDate = fromDate;
        _toDate = toDate;
    }
    
    public override Expression<Func<Proof, bool>> ToExpression()
    {
        return x => (!_fromDate.HasValue || x.Created >= _fromDate) && (!_toDate.HasValue || x.Created <= _toDate);
    }
}