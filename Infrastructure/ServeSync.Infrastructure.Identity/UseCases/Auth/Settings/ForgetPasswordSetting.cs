namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Settings;

public class ForgetPasswordSetting
{
    public int ExpiresInMinute { get; set; }
    public string[] AllowedClients { get; set; }
}