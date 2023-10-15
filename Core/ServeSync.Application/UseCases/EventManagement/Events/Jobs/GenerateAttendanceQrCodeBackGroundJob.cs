using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class GenerateAttendanceQrCodeBackGroundJob : IBackGroundJob
{
    public Guid EventId { get; set; }
    
    public GenerateAttendanceQrCodeBackGroundJob(Guid eventId)
    {
        EventId = eventId;
    }
}