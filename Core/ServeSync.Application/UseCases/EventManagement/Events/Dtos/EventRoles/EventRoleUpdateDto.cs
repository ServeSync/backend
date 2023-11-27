using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;

public class EventRoleUpdateDto
{
    public Guid? Id { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MinLength(10)]
    public string Description { get; set; } = null!;
    
    [Required]
    public bool IsNeedApprove { get; set; }
    
    [Required]
    public double Score { get; set; }
    
    [Required]
    public int Quantity { get; set; }
}