using Newtonsoft.Json;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

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
    public int Key { get; set; }
    
    [JsonIgnore]
    public int SubKey { get; set; }
}