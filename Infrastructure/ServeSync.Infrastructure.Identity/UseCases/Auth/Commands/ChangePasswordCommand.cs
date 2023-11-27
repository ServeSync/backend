using ServeSync.Application.SeedWorks.Cqrs;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class ChangePasswordCommand : ICommand
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; } 
    
    public ChangePasswordCommand(string currentPassword, string newPassword)
    {
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }
}