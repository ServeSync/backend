using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class RejectEventOrganizationInvitationCommand : ICommand
{
    public string Code { get; set; }
    
    public RejectEventOrganizationInvitationCommand(string code)
    {
        Code = code;
    }
}