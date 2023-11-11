using System.Security.Claims;
using Microsoft.Extensions.Options;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Settings;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthCredentialDto>
{
    private readonly JwtSetting _jwtSetting;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RefreshTokenCommandHandler(
        IOptions<JwtSetting> jwtOptions,
        ITokenProvider tokenProvider,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _jwtSetting = jwtOptions.Value;
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AuthCredentialDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByRefreshTokenAsync(request.RefreshToken);
        if (user == null)
        {
            throw new RefreshTokenNotFoundException(request.RefreshToken);
        }

        var accessTokenId = string.Empty;
        if (_tokenProvider.ValidateToken(request.AccessToken, ref accessTokenId))
        {
            throw new AccessTokenStillValidException();
        }
        
        user.UseRefreshToken(accessTokenId, request.RefreshToken);
        
        var accessToken = _tokenProvider.GenerateAccessToken(GetUserAuthenticateClaimsAsync(user));
        var credential = new AuthCredentialDto()
        {
            AccessToken = accessToken.Value,
            RefreshToken = _tokenProvider.GenerateRefreshToken()
        };
            
        user.AddRefreshToken(accessToken.Id, credential.RefreshToken, DateTime.UtcNow.AddDays(_jwtSetting.RefreshTokenExpiresInDay));
            
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return credential;
    }
    
    private IEnumerable<Claim> GetUserAuthenticateClaimsAsync(ApplicationUser user)
    {
        var claims = new List<Claim>()
        {
            new (AppClaim.UserId, user.Id),
            new (AppClaim.UserName, user.UserName),
            new (AppClaim.Email, user.Email)
        };
        
        return claims;
    }
}