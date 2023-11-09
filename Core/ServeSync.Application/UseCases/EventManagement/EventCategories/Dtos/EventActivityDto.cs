namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

public class BasicEventActivityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public class EventActivityDto : BasicEventActivityDto
{
    public double MinScore { get; set; }
    public double MaxScore { get; set; }
    public Guid EventCategoryId { get; set; }
}

public class EventActivityDetailDto : EventActivityDto
{
    public string EventCategoryName { get; set; } = null!;
}