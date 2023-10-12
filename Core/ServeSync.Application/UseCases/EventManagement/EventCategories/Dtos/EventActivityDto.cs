namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;

public class EventActivityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double MinScore { get; set; }
    public double MaxScore { get; set; }
    public Guid EventCategoryId { get; set; }
}