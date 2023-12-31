﻿namespace ServeSync.Application.Common.Settings;

public class JwtSetting
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Key { get; set; }
    public int ExpiresInMinute { get; set; }
    public int RefreshTokenExpiresInDay { get; set; }
}