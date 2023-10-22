using System.ComponentModel.DataAnnotations;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

public class SignInDto
{
    [Required] 
    public string UserNameOrEmail { get; set; } = null!;

    [Required] 
    public string Password { get; set; } = null!;
}