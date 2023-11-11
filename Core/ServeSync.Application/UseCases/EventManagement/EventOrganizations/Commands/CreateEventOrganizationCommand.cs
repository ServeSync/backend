using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class CreateEventOrganizationCommand : ICommand<Guid>
{
    public EventOrganizationCreateDto EventOrganization { get; set; }
    
    public CreateEventOrganizationCommand(EventOrganizationCreateDto eventOrganizationCreateDto)
    {
        EventOrganization = eventOrganizationCreateDto;
    }
    
}