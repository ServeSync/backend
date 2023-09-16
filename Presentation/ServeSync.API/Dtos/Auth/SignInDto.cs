using System.ComponentModel.DataAnnotations;

namespace ServeSync.API.Dtos.Auth;

public class SignInDto
{
    [Required]
    public string UserNameOrEmail { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}