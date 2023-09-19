using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.MailSender.Interfaces;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Settings;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class ResetPasswordByTokenCommandHandler : ICommandHandler<ResetPasswordByTokenCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public ResetPasswordByTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }
    
    public async Task Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
    {
        var deserializedToken = DeserializeToken(request.Token);
        
        var user = await _userRepository.FindByIdAsync(deserializedToken.UserId);
        if (user == null)
        {
            throw new InvalidForgetPasswordTokenException();
        }
        
        var result = await _userManager.ResetPasswordAsync(user, deserializedToken.Value, request.Password);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            throw new ResourceInvalidOperationException(error.Description, error.Code);
        }
    }
    
    private ForgetPasswordTokenDto DeserializeToken(string token)
    {
        try
        {
            var deserializedToken = Encryptor.Base64Decode<ForgetPasswordTokenDto>(token);
            return deserializedToken;
        }
        catch 
        {
            throw new InvalidForgetPasswordTokenException();
        }
    }
}