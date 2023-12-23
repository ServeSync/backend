using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.Statistics.Dtos;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetProofStatisticQuery : IQuery<ProofStatisticDto>
{
    public RecurringFilterType? Type { get; set; }
    
    public GetProofStatisticQuery(RecurringFilterType? type)
    {
        Type = type;
    }
}