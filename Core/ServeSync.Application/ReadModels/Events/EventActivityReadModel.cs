using ServeSync.Application.ReadModels.Abstracts;

namespace ServeSync.Application.ReadModels.Events;

public class EventActivityReadModel : BaseReadModel<Guid>
{
    public string Name { get; set; } = null!;
    public double MinScore { get; set; }
    public double MaxScore { get; set; }
    public Guid EventCategoryId { get; set; }
    public string EventCategoryName { get; set; } = null!;
}