using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetEventRegisteredStudentStatisticQuery : IQuery<List<EventStudentStatisticDto>>
{
    public TimeType TimeType { get; set; }
    public int NumberOfRecords { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    
    public GetEventRegisteredStudentStatisticQuery(TimeType timeType, int numberOfRecords, DateTime? startAt, DateTime? endAt)
    {
        TimeType = timeType;
        NumberOfRecords = numberOfRecords;
        
        if (TimeType is TimeType.Custom && (startAt is null || endAt is null))
            throw new ResourceInvalidDataException("StartAt and EndAt must be not null when TimeType is Custom");

        StartAt = startAt;
        EndAt = endAt;
    }
}