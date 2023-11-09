using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

public class EventActivityByCategoryFilterRequestDto : PagingAndSortingRequestDto
{
    public string? Search { get; set; }

    [SortConstraint(Fields = $"{nameof(EventActivity.Name)}, {nameof(EventActivity.MinScore)}, {nameof(EventActivity.MaxScore)}")]
    public override string? Sorting { get; set; } = string.Empty;
}