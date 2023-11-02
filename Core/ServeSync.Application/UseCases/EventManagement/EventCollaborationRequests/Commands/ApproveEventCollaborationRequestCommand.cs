using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.EventCollaborationRequests.Commands;

public class ApproveEventCollaborationRequestCommand : ICommand<Guid>
{
    public Guid Id { get; set; }
    
    public ApproveEventCollaborationRequestCommand(Guid id)
    {
        Id = id;
    }
}