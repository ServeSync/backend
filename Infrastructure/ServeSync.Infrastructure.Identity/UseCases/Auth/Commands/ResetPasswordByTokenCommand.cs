using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class ResetPasswordByTokenCommand : ICommand
{
    public string Token { get; set; }
    
    public string Password { get; set; }
    
    public ResetPasswordByTokenCommand(string token, string password)
    {
        Token = token;
        Password = password;
    }
}