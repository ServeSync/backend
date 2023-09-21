using MediatR;
using ServeSync.Application.Identity;
using ServeSync.Infrastructure.Identity.UseCases.Permissions.Queries;

namespace ServeSync.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IMediator _mediator;
    
    public IdentityService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<IEnumerable<string>> GetPermissionsForUserAsync(string userId)
    {
        var permissions = await _mediator.Send(new GetAllPermissionForUserQuery(userId));
        return permissions.Select(x => x.Name);
    }
}