using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

public class EventCategoryFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }

    [SortConstraint(Fields = $"{nameof(EventCategory.Name)}")]
    public override string? Sorting { get; set; } = string.Empty;
}