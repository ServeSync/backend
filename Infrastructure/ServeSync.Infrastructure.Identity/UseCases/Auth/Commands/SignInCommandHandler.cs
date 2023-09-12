using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class SignInCommandHandler : ICommandHandler<SignInCommand, AuthCredentialDto>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;

    public SignInCommandHandler(
        SignInManager<ApplicationUser> signInManager,
        ITokenProvider tokenProvider,
        IUserRepository userRepository)
    {
        _signInManager = signInManager;
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
    }
    
    public async Task<AuthCredentialDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUserNameOrEmailAsync(request.UserNameOrEmail, request.UserNameOrEmail);
        if (user == null)
        {
            throw new UserNameOrEmailNotFoundException(request.UserNameOrEmail);
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (result.Succeeded)
        {
            return new AuthCredentialDto()
            {
                AccessToken = _tokenProvider.GenerateAccessToken(GetUserAuthenticateClaimsAsync(user)),
                RefreshToken = string.Empty
            };
        }

        if (result.IsLockedOut)
        {
            throw new AccountLockedOutException();
        }

        throw new InvalidCredentialException();
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