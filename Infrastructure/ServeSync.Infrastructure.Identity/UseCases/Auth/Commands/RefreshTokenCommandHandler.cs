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
using ServeSync.Infrastructure.Identity.Services;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthCredentialDto>
{
    private readonly JwtSetting _jwtSetting;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IdentityUserClaimGenerator _identityUserClaimGenerator;
    
    public RefreshTokenCommandHandler(
        IOptions<JwtSetting> jwtOptions,
        ITokenProvider tokenProvider,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IdentityUserClaimGenerator identityUserClaimGenerator)
    {
        _jwtSetting = jwtOptions.Value;
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _identityUserClaimGenerator = identityUserClaimGenerator;
    }
    
    public async Task<AuthCredentialDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByRefreshTokenAsync(request.RefreshToken);
        if (user == null)
        {
            throw new RefreshTokenNotFoundException(request.RefreshToken);
        }

        var accessTokenId = string.Empty;
        var claims = new List<Claim>();
        if (_tokenProvider.ValidateToken(request.AccessToken, ref accessTokenId, ref claims))
        {
            throw new AccessTokenStillValidException();
        }
        
        user.UseRefreshToken(accessTokenId, request.RefreshToken);

        var tenantId = claims.FirstOrDefault(x => x.Type == AppClaim.TenantId)?.Value;
        var accessToken = _tokenProvider.GenerateAccessToken(await _identityUserClaimGenerator.GenerateAsync(user, tenantId == null ? null : Guid.Parse(tenantId)));
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
}