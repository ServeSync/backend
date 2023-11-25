using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Jobs;

public class SyncEventOrganizationContactReadModelBackGroundJob : IBackGroundJob
{
    public Guid EventOrganizationContactId { get; set; }
    
    public SyncEventOrganizationContactReadModelBackGroundJob(Guid eventOrganizationContactId)
    {
        EventOrganizationContactId = eventOrganizationContactId;
    }
}