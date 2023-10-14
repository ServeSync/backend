using System.Text.Json.Serialization;
using ServeSync.API.Common.Validations;
using ServeSync.Application.Common.Dtos;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;

namespace ServeSync.API.Dtos.Events;

public class EventFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventType? EventType { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventStatus? EventStatus { get; set; }

    [SortConstraint(Fields = $"{nameof(Event.Name)}, {nameof(Event.Address)}, {nameof(Event.StartAt)}, {nameof(Event.EndAt)}, {nameof(Event.Type)}, RepresentativeOrganization")]
    public override string? Sorting { get; set; } = string.Empty;
}