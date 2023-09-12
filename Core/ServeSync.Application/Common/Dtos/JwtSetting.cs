namespace ServeSync.Application.Common.Dtos;

public class JwtSetting
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Key { get; set; }
    public int Expires { get; set; }
}