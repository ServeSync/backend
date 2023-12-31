﻿using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Infrastructure.Identity.Models.RoleAggregate.Exceptions;

public class DefaultRoleAccessDeniedException : ResourceInvalidOperationException
{
    public DefaultRoleAccessDeniedException(string role) 
        : base($"Can not create, update or delete '{role}' role!", ErrorCodes.DefaultRoleAccessDenied)
    {
    }
}