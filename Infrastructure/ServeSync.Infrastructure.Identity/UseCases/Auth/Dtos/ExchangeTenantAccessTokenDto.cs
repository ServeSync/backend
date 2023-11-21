using System.ComponentModel.DataAnnotations;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

public class ExchangeTenantAccessTokenDto
{
    [Required]
    public Guid TenantId { get; set; }
}