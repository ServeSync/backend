namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

public class AuthCredentialDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}