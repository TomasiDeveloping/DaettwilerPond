using Application.Interfaces;
using Application.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services;

public class EmailService(IOptions<EmailConfiguration> emailConfiguration, ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration = emailConfiguration.Value;

    public async Task<bool> SendEmailAsync(EmailMessage message)
    {
        var mimeMessage = CreateEmailMessage(message);
        return await SendMailAsync(mimeMessage);
    }

    public async Task<bool> SendFishingLicenseBillAsync(int fishingLicenseYear, string userEmail, string mailContent,
        byte[] qrBill)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Dättwiler Weiher", _emailConfiguration.From));
        emailMessage.To.Add(new MailboxAddress(userEmail, userEmail));
        emailMessage.Subject = $"Rechnung Fischerkarte {fishingLicenseYear}";
        var bodyBuilder = new BodyBuilder
        {
            TextBody = mailContent
        };
        bodyBuilder.Attachments.Add($"Rechnung_Fischerkarte_{fishingLicenseYear}.pdf", qrBill,
            new ContentType("application", "pdf"));
        emailMessage.Body = bodyBuilder.ToMessageBody();
        return await SendMailAsync(emailMessage);
    }

    public async Task<bool> SendEmailToMembersAsync(List<string> recipientAddresses, string subject, string mailContent, IFormFileCollection? attachments)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress("Dättwiler Weiher", _emailConfiguration.From));
        mailMessage.Subject = subject;
        mailMessage.To.AddRange(recipientAddresses.Select(x => new MailboxAddress(x, x)));

        var bodyBuilder = new BodyBuilder()
        {
            TextBody = mailContent
        };
        if (attachments is not null && attachments.Any())
        {
            foreach (var attachment in attachments)
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await attachment.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }
        }
        mailMessage.Body = bodyBuilder.ToMessageBody();

        return await SendMailAsync(mailMessage);
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
            logger.LogError(e, e.Message);
            return false;
        }
        finally
        {
            await client.DisconnectAsync(true);
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