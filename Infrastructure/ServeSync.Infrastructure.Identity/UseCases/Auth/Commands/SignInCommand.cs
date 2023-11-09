using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Enums;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class SignInCommand : ICommand<AuthCredentialDto>
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
    public LoginPortal LoginPortal { get; set; }

    public SignInCommand(string userNameOrEmail, string password, LoginPortal loginPortal)
    {
        UserNameOrEmail = userNameOrEmail;
        Password = password;
        LoginPortal = loginPortal;
    }
}