using System.Security.Claims;
using ServeSync.Application.Common.Dtos;

namespace ServeSync.Application.Services.Interfaces;

public interface ITokenProvider
{
    AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
    
    string GenerateRefreshToken();

    bool ValidateToken(string token, ref string id);
}