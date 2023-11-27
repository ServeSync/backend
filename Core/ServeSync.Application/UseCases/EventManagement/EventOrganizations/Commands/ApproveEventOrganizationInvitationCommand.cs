using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class ApproveEventOrganizationInvitationCommand : ICommand
{
    public string Code { get; set; }
    
    public ApproveEventOrganizationInvitationCommand(string code)
    {
        Code = code;
    }
}