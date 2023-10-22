using System.ComponentModel.DataAnnotations;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

public class ResetPasswordDto
{
    [Required] 
    public string Token { get; set; } = null!;

    [Required] 
    [MinLength(6)] 
    public string NewPassword { get; set; } = null!;
}