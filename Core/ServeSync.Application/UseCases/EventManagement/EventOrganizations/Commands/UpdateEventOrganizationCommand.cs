using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventOrganizations.Dtos;

namespace ServeSync.Application.UseCases.EventManagement.EventOrganizations.Commands;

public class UpdateEventOrganizationCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string ImageUrl { get; set; }
    
    public UpdateEventOrganizationCommand(
        Guid id,
        string name,
        string? description,
        string email,
        string phoneNumber,
        string? address,
        string imageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        ImageUrl = imageUrl;
    }
}