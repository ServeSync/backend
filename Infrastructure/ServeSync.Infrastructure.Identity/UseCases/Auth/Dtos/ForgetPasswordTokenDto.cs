namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

public class ForgetPasswordTokenDto
{
    public string Value { get; set; } = null!;
    public string UserId { get; set; } = null!;
}