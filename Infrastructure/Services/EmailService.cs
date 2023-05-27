using Application.Interfaces;
using Application.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailConfiguration> emailConfiguration, ILogger<EmailService> logger)
    {
        _emailConfiguration = emailConfiguration.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(EmailMessage message)
    {
        var mimeMessage = CreateEmailMessage(message);
        return await SendMailAsync(mimeMessage);
    }

    private async Task<bool> SendMailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
            await client.SendAsync(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return false;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }

        return true;
    }

    private MimeMessage CreateEmailMessage(EmailMessage message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Dättwiler Weiher", _emailConfiguration.From));
        emailMessage.To.AddRange(message.To.Select(x => new MailboxAddress(x, x)));
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(TextFormat.Html) {Text = message.Content};
        return emailMessage;
    }
}