using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.MailSender;
using ServeSync.Application.SeedWorks.MailSender.Interfaces;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.UseCases.Auth.Commands;

public class RequestForgetPasswordTokenCommandHandler : ICommandHandler<RequestForgetPasswordTokenCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateGenerator _emailTemplateGenerator;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public RequestForgetPasswordTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        IEmailSender emailSender,
        IEmailTemplateGenerator emailTemplateGenerator)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _emailTemplateGenerator = emailTemplateGenerator;
        _userManager = userManager;
    }
    
    public async Task Handle(RequestForgetPasswordTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUserNameOrEmailAsync(request.UserNameOrEmail, request.UserNameOrEmail);
        if (user == null)
        {
            throw new UserNameOrEmailNotFoundException(request.UserNameOrEmail);
        }
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callBackUrlWithToken = QueryHelpers.AddQueryString(request.CallBackUrl, new Dictionary<string, string>()
        {
            {"token", token }
        });

        await _emailSender.SendAsync(new EmailMessage()
        {
            ToAddress = user.Email,
            Subject = "[ServeSync] Thay đổi mật khẩu",
            Body = _emailTemplateGenerator.GetForForgetPassword(callBackUrlWithToken)
        });
    }
}