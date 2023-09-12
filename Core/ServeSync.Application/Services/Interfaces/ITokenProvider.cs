using System.Security.Claims;

namespace ServeSync.Application.Services.Interfaces;

public interface ITokenProvider
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
}