using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class SignInCommand : ICommand<AuthCredentialDto>
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }

    public SignInCommand(string userNameOrEmail, string password)
    {
        UserNameOrEmail = userNameOrEmail;
        Password = password;
    }
}