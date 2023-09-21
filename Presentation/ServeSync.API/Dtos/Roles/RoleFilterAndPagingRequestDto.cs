﻿using ServeSync.API.Common.Validations;
using ServeSync.Application.Common.Dtos;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;

namespace ServeSync.API.Dtos.Roles;

public class RoleFilterAndPagingRequestDto : PagingAndSortingRequestDto
{
    public string Name { get; set; } = string.Empty;

    [SortConstraint(Fields = $"{nameof(ApplicationRole.Name)}")]
    public override string Sorting { get; set; } = string.Empty;
}