using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Validations;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.UseCases.Users.Dtos;

public class UserRequestDto: PagingAndSortingRequestDto
{
    public string? Search { get; set; }

    [SortConstraint(Fields = $"{nameof(ApplicationUser.UserName)}, {nameof(ApplicationUser.Email)}")]
    public override string? Sorting { get; set; } = string.Empty;
}