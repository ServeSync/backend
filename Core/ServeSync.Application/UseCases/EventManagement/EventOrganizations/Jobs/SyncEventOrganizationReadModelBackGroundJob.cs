using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Jobs;

public class SyncEventOrganizationReadModelBackGroundJob : IBackGroundJob
{
    public Guid EventOrganizationId { get; set; }
    
    public SyncEventOrganizationReadModelBackGroundJob(Guid eventOrganizationId)
    {
        EventOrganizationId = eventOrganizationId;
    }
}