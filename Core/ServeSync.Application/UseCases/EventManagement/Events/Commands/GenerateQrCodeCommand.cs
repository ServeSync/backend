using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class GenerateQrCodeCommand : ICommand
{
    public Guid[] Ids { get; set; }
    
    public GenerateQrCodeCommand(Guid[] ids)
    {
        Ids = ids;
    }
}