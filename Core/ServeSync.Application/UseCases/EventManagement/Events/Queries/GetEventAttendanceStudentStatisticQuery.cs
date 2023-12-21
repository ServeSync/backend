using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetEventAttendanceStudentStatisticQuery : IQuery<List<EventStudentStatisticDto>>
{
    public TimeType TimeType { get; set; }
    public int NumberOfRecords { get; set; }
    
    public GetEventAttendanceStudentStatisticQuery(TimeType timeType, int numberOfRecords)
    {
        TimeType = timeType;
        NumberOfRecords = numberOfRecords;
    }
}