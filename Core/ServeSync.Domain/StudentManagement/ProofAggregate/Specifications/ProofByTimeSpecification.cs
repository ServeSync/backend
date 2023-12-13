using System.Linq.Expressions;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

public class ProofByTimeSpecification : Specification<Proof, Guid>
{
    private readonly DateTime? _fromDate;
    private readonly DateTime? _toDate;
    
    public ProofByTimeSpecification(DateTime? fromDate, DateTime? toDate)
    {
        _fromDate = fromDate;
        _toDate = toDate;
    }
    
    public override Expression<Func<Proof, bool>> ToExpression()
    {
        return x => (
                        x.ProofType == ProofType.External
                        && (
                            (_fromDate.HasValue || x.ExternalProof!.AttendanceAt >= _fromDate)
                            && (_toDate.HasValue || x.ExternalProof!.AttendanceAt <= _toDate)
                        )
                    ) ||
                    (
                        x.ProofType == ProofType.Internal
                        && (
                            (_fromDate.HasValue || x.InternalProof!.AttendanceAt >= _fromDate)
                            && (_toDate.HasValue || x.InternalProof!.AttendanceAt <= _toDate)
                        )
                    ) ||
                    (
                        x.ProofType == ProofType.Special
                        && (
                            (_fromDate.HasValue || x.Created >= _fromDate)
                            && (_toDate.HasValue || x.Created <= _toDate)
                        )
                    );
    }
}