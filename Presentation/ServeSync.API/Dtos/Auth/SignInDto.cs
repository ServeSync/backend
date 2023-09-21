using System.ComponentModel.DataAnnotations;

namespace ServeSync.API.Dtos.Auth;

public class SignInDto
{
    [Required]
    public string UserNameOrEmail { get; set; }
    
    [Required]
    public string Password { get; set; }
}