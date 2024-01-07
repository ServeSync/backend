using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Application.UseCases.Statistics.Dtos;

public class ProofStatisticDto
{
    public int Total { get; set; }
    public List<ProofStatisticRecordDto> Data { get; set; } = null!;
}

public class ProofStatisticRecordDto
{
    public ProofStatus Status { get; set; }
    public int Count { get; set; }
}
