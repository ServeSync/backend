using Newtonsoft.Json;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.Statistics.Dtos;

public class EventStatisticDto
{
    public int Total { get; set; }
    public List<EventStatisticRecordDto> Data { get; set; } = null!;
}

public class EventStatisticRecordDto
{
    public EventStatus Status { get; set; }
    public int Count { get; set; }
}

public class EventStudentStatisticDto
{
    public string Name { get; set; } = null!;
    public int Value { get; set; }
    
    [JsonIgnore]
    public int Day { get; set; }
    
    [JsonIgnore]
    public int Month { get; set; }
    
    [JsonIgnore]
    public int Year { get; set; }
}