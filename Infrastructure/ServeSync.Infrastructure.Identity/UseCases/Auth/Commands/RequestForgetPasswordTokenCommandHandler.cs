using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.MailSender;
using ServeSync.Application.SeedWorks.MailSender.Interfaces;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Settings;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class RequestForgetPasswordTokenCommandHandler : ICommandHandler<RequestForgetPasswordTokenCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly ForgetPasswordSetting _forgetPasswordSetting;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public RequestForgetPasswordTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IOptions<ForgetPasswordSetting> forgetPasswordSetting,
        IUserRepository userRepository,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _userManager = userManager;
        _forgetPasswordSetting = forgetPasswordSetting.Value;
    }
    
    public async Task Handle(RequestForgetPasswordTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_forgetPasswordSetting.AllowedClients.Contains(request.CallBackUrl))
        {
            throw new ResourceInvalidOperationException("CallBackUrl is not allowed!");
        }
        
        var user = await _userRepository.FindByUserNameOrEmailAsync(request.UserNameOrEmail, request.UserNameOrEmail);
        if (user == null)
        {
            throw new UserNameOrEmailNotFoundException(request.UserNameOrEmail);
        }

        var token = new ForgetPasswordTokenDto()
        {
            Value = await _userManager.GeneratePasswordResetTokenAsync(user),
            UserId = user.Id
        };
        
        var callBackUrlWithToken = QueryHelpers.AddQueryString(request.CallBackUrl, new Dictionary<string, string>()
        {
            {"token", Encryptor.Base64Encode<ForgetPasswordTokenDto>(token) }
        });

        await _emailSender.SendAsync(new EmailMessage()
        {
            ToAddress = user.Email,
            Subject = "[ServeSync] Thay đổi mật khẩu",
            Body = _emailTemplateGenerator.GetForForgetPassword(callBackUrlWithToken)
        });
    }
}