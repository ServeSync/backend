namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos
{
    public class EventOrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string ImageUrl { get; set; } = null!;
    }

    public class EventOrganizationDetailDto : EventOrganizationDto
    {
        public List<EventOrganizationContactDto> Contacts { get; set; } = null!;
    }
}