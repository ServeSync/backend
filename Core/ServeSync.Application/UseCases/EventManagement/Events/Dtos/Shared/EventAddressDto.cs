using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.Shared;

public class EventAddressDto
{
    [Required]
    public string FullAddress { get; set; } = null!;
    
    [Required]
    public double Longitude { get; set; }
    
    [Required]
    public double Latitude { get; set; }
}