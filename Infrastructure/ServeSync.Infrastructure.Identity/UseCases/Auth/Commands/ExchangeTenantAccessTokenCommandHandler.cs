using Microsoft.Extensions.Options;
using ServeSync.Application.Common.Settings;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.Services;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class ExchangeTenantAccessTokenCommandHandler : ICommandHandler<ExchangeTenantAccessTokenCommand, AuthCredentialDto>
{
    private readonly JwtSetting _jwtSetting;
    private readonly ICurrentUser _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    
    public ExchangeTenantAccessTokenCommandHandler(
        IOptions<JwtSetting> jwtOptions,
        ICurrentUser currentUser, 
        IUserRepository userRepository,
        ITokenProvider tokenProvider,
        IUnitOfWork unitOfWork)
    {
        _jwtSetting = jwtOptions.Value;
        _currentUser = currentUser;
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AuthCredentialDto> Handle(ExchangeTenantAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(_currentUser.Id);
        if (user == null)
        {
            throw new UserNotFoundException(_currentUser.Id);
        }
        
        var accessToken = _tokenProvider.GenerateAccessToken(IdentityUserClaimGenerator.Generate(user, request.TenantId));
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