﻿using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;

public class RejectEventCollaborationRequestCommand : ICommand
{
    public Guid Id { get; set; }
    
    public RejectEventCollaborationRequestCommand(Guid id)
    {
        Id = id;
    }
}