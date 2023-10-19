using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class SendMailCancelledEventToStudentBackGroundJob : IBackGroundJob
{
    public string EventName { get; set; }
    public List<string> Emails { get; set; }

    public SendMailCancelledEventToStudentBackGroundJob(string eventName, List<string> emails)
    {
        EventName = eventName;
        Emails = emails;
    }
}