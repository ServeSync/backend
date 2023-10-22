using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.UseCases.Roles.Dtos;

public class RoleFilterAndPagingRequestDto : PagingAndSortingRequestDto
{
    public string Name { get; set; } = string.Empty;

    [SortConstraint(Fields = $"{nameof(ApplicationRole.Name)}")]
    public override string Sorting { get; set; } = string.Empty;
}