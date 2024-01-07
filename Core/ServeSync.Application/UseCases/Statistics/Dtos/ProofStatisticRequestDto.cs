using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.UseCases.Statistics.Dtos;

public class ProofStatisticRequestDto
{
    public RecurringFilterType? Type { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
}