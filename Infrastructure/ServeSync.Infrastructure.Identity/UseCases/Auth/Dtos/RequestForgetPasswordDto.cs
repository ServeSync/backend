using System.ComponentModel.DataAnnotations;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

public class RequestForgetPasswordDto
{
    [Required] 
    public string UserNameOrEmail { get; set; } = null!;

    [Required] 
    public string CallBackUrl { get; set; } = null!;
}