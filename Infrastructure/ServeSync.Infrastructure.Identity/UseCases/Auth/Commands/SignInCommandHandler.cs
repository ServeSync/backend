﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ServeSync.Application.Common;
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
using ServeSync.Infrastructure.Identity.UseCases.Auth.Enums;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class SignInCommandHandler : ICommandHandler<SignInCommand, AuthCredentialDto>
{
    private readonly JwtSetting _jwtSetting;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IdentityUserClaimGenerator _identityUserClaimGenerator;

    public SignInCommandHandler(
        IOptions<JwtSetting> jwtOptions,
        SignInManager<ApplicationUser> signInManager,
        ITokenProvider tokenProvider,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IdentityUserClaimGenerator identityUserClaimGenerator)
    {
        _jwtSetting = jwtOptions.Value;
        _signInManager = signInManager;
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _identityUserClaimGenerator = identityUserClaimGenerator;
    }
    
    public async Task<AuthCredentialDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserByPortalAsync(request.UserNameOrEmail, request.LoginPortal);
        if (user == null)
        {
            throw new UserNameOrEmailNotFoundException(request.UserNameOrEmail);
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (result.Succeeded)
        {
            var accessToken = _tokenProvider.GenerateAccessToken(await _identityUserClaimGenerator.GenerateAsync(user));
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

        if (result.IsLockedOut)
        {
            throw new AccountLockedOutException();
        }

        throw new InvalidCredentialException();
    }
    
    private Task<ApplicationUser?> GetUserByPortalAsync(string userNameOrEmail, LoginPortal loginPortal)
    {
        if (loginPortal == LoginPortal.Admin)
        {
            return _userRepository.FindByUserNameOrEmailAndRoles(userNameOrEmail, userNameOrEmail, new List<string>()
            {
                AppRole.Admin,
                AppRole.StudentAffair,
                AppRole.EventOrganization,
                AppRole.EventOrganizer
            });
        }
        
        return _userRepository.FindByUserNameOrEmailAndRoles(userNameOrEmail, userNameOrEmail, new List<string>()
        {
            AppRole.Student
        });
    }
}