﻿using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServeSync.Application.Common.Settings;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Infrastructure.Gmail;

public class GmailSender : IEmailSender
{
    private readonly EmailSetting _emailSetting;
    private readonly ILogger<GmailSender> _logger;
    private readonly IBackGroundJobManager _backGroundJobManager;
    
    public GmailSender(
        IOptions<EmailSetting> emailSetting,
        ILogger<GmailSender> logger,
        IBackGroundJobManager backGroundJobManager)
    {
        _emailSetting = emailSetting.Value;
        _logger = logger;
        _backGroundJobManager = backGroundJobManager;
    }
    
    public async Task SendAsync(EmailMessage emailMessage)
    {
        try
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_emailSetting.UserName, _emailSetting.From),
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = true
            };
            
            message.To.Add(new MailAddress(emailMessage.ToAddress));
        
            var client = new SmtpClient
            {
                Port = _emailSetting.Port,
                EnableSsl = true,
                Host = _emailSetting.SmtpServer,
            };
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_emailSetting.UserName, _emailSetting.Password);
            
            await client.SendMailAsync(message);
        }
        catch (Exception e)
        {
            _logger.LogError("Send email {Subject} failed to {ToAddress}: {Message}!", emailMessage.Subject, emailMessage.ToAddress, e.Message);
        }
    }

    public void Push(EmailMessage emailMessage)
    {
        _backGroundJobManager.Fire(() => SendAsync(emailMessage));
    }
}