namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.OrganizationInEvents;

public class BasicOrganizationInEventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    public string ImageUrl { get; set; } = null!;
    public Guid OrganizationId { get; set; }
}

public class OrganizationInEventDto : BasicOrganizationInEventDto
{
    public string Role { get; set; } = null!;
    public List<BasicRepresentativeInEventDto> Representatives { get; set; } = null!;
}

public class BasicRepresentativeInEventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? Position { get; set; }
    public string Role { get; set; } = null!;
    public Guid OrganizationRepId { get; set; }
}