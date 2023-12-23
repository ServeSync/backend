using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.Statistics.Dtos;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetEventRegisteredStudentStatisticQuery : IQuery<List<EventStudentStatisticDto>>
{
    public TimeType TimeType { get; set; }
    public int NumberOfRecords { get; set; }
    
    public GetEventRegisteredStudentStatisticQuery(TimeType timeType, int numberOfRecords)
    {
        TimeType = timeType;
        NumberOfRecords = numberOfRecords;
    }
}