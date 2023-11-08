using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Queries;

public class GetEventOrganizationByIdQuery : IQuery<EventOrganizationDetailDto>
{
    public Guid OrganizationId { get; set; }
    
    public GetEventOrganizationByIdQuery(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
    
}