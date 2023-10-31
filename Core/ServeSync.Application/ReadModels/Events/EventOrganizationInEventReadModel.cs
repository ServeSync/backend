using ServeSync.Application.ReadModels.Abstracts;

namespace ServeSync.Application.ReadModels.Events;

public class EventOrganizationInEventReadModel : BaseReadModel<Guid>
{
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public Guid OrganizationId { get; set; }
    public string Role { get; set; } = null!;
    public List<EventOrganizationRepresentativeInEventReadModel> Representatives { get; set; } = null!;
}

public class BasicEventOrganizationInEventReadModel : BaseReadModel<Guid>
{
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    public Guid OrganizationId { get; set; }
}

public class EventOrganizationRepresentativeInEventReadModel : BaseReadModel<Guid>
{
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? Position { get; set; }
    public string Role { get; set; } = null!;
    public Guid OrganizationRepId { get; set; }
}