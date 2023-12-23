using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetProofStatisticQueryHandler : IQueryHandler<GetProofStatisticQuery, ProofStatisticDto>
{
    private readonly IBasicReadOnlyRepository<Proof, Guid> _proofRepository;
    
    public GetProofStatisticQueryHandler(IBasicReadOnlyRepository<Proof, Guid> proofRepository)
    {
        _proofRepository = proofRepository;
    }
    
    public async Task<ProofStatisticDto> Handle(GetProofStatisticQuery request, CancellationToken cancellationToken)
    {
        var statisticStatuses = new List<ProofStatus>()
        {
            ProofStatus.Pending,
            ProofStatus.Approved,
            ProofStatus.Rejected
        };

        var dateTime = DateTime.UtcNow;
        
        var specification = GetSpecificationByRecurringType(request.Type, dateTime);
        var result = new ProofStatisticDto()
        {
            Total = await _proofRepository.GetCountAsync(specification),
            Data = new List<ProofStatisticRecordDto>()
        };
        
        foreach (var status in statisticStatuses)
        {
            var eventStatusSpecification = specification.And(new ProofByStatusSpecification(status));

            result.Data.Add(new ProofStatisticRecordDto()
            {
                Status = status,
                Count = await _proofRepository.GetCountAsync(eventStatusSpecification)
            });
        }

        return result;
    }
    
    private ISpecification<Proof, Guid> GetSpecificationByRecurringType(RecurringFilterType? type, DateTime dateTime)
    {
        if (!type.HasValue)
        {
            return EmptySpecification<Proof, Guid>.Instance;
        }
        
        var (startAt, endAt) = DateTimeHelper.GetByRecurringType(type.Value, dateTime);
        return new ProofByCreateTimeSpecification(startAt, endAt);
    }
}