using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class RefreshTokenCommand : ICommand<AuthCredentialDto>
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }

    public RefreshTokenCommand(string refreshToken, string accessToken)
    {
        RefreshToken = refreshToken;
        AccessToken = accessToken;
    }
}