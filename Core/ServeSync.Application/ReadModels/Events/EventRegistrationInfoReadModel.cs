using ServeSync.Application.ReadModels.Abstracts;

namespace ServeSync.Application.ReadModels.Events;

public class EventRegistrationInfoReadModel : BaseReadModel<Guid>
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}