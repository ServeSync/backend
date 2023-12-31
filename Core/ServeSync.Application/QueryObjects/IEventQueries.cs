﻿using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;

namespace ServeSync.Application.QueryObjects;

public interface IEventQueries
{
    Task<EventReadModel?> GetEventReadModelByIdAsync(Guid eventId);
}