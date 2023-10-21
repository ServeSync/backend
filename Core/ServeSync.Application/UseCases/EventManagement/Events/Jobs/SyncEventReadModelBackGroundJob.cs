using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class SyncEventReadModelBackGroundJob : IBackGroundJob
{
    public Guid EventId { get; set; }
    
    public SyncEventReadModelBackGroundJob(Guid eventId)
    {
        EventId = eventId;
    }
}