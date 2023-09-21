using System.ComponentModel.DataAnnotations;

namespace ServeSync.API.Dtos.Auth;

public class RequestForgetPasswordDto
{
    [Required]
    public string UserNameOrEmail { get; set; }
    
    [Required]
    public string CallBackUrl { get; set; }
}