using System.Text.Json.Serialization;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Dtos;

public class EventCollaborationRequestFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventType? Type { get; set; }
    
    [SortConstraint(Fields = $"{nameof(EventCollaborationRequest.Name)}, {nameof(EventCollaborationRequest.StartAt)}, {nameof(EventCollaborationRequest.EndAt)}, {nameof(EventCollaborationRequest.Type)}")]
    public override string? Sorting { get; set; } = string.Empty;
}