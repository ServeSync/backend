using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Settings;
using ServeSync.Application.Services.Interfaces;

namespace ServeSync.Application.Services;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtSetting _jwtSetting;

    public JwtTokenProvider(IOptions<JwtSetting> jwtOptions)
    {
        _jwtSetting = jwtOptions.Value;
    }

    public AccessToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSetting.Key);
        var token = new JwtSecurityToken(
            issuer: _jwtSetting.Issuer,
            audience: _jwtSetting.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiresInMinute),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );
        
        token.Payload.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return new AccessToken()
        {
            Id = token.Payload.Jti,
            Value = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
    
    public bool ValidateToken(string token, ref string id)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            var key = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var tokenValidationParam = GetTokenValidationParam();
            var tokenInVerification = jwtTokenHandler.ValidateToken(token, tokenValidationParam, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        finally
        {
            id = jwtTokenHandler.ReadJwtToken(token).Payload.Jti;
        }
    }

    private TokenValidationParameters GetTokenValidationParam()
    {
        return new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
        
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key)),
        };
    }
}