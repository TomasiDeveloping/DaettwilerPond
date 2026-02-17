#nullable enable
using Application.Interfaces;
using Application.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services;

// Service for sending emails
public class EmailService(IOptions<EmailConfiguration> emailConfiguration, ILogger<EmailService> logger) : IEmailService
{
    // Email configuration settings injected via dependency injection
    private readonly EmailConfiguration _emailConfiguration = emailConfiguration.Value;

    // Implementation of sending a general email
    public async Task<bool> SendEmailAsync(EmailMessage message)
    {
        var mimeMessage = CreateEmailMessage(message);
        return await SendMailAsync(mimeMessage);
    }

    // Implementation of sending a fishing license bill via email
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

    // Implementation of sending emails to multiple members
    public async Task<bool> SendEmailToMembersAsync(List<string> recipientAddresses, string subject, string mailContent,
        IFormFileCollection? attachments)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress("Dättwiler Weiher", _emailConfiguration.From));
        mailMessage.Subject = subject;
        mailMessage.To.AddRange(recipientAddresses.Select(x => new MailboxAddress(x, x)));

        // Information text about automated response
        const string infoText =
            "<b style=\"color: red;\">Bitte beachte, dass diese E-Mail von einer automatischen Absenderadresse versandt wird. Auf Antworten an diese Nachricht wird nicht reagiert.</b> ";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"{mailContent.ReplaceLineEndings("<br>")} <br><br> {infoText}"
        };

        // Attachments handling
        if (attachments is not null && attachments.Any())
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

        mailMessage.Body = bodyBuilder.ToMessageBody();

        return await SendMailAsync(mailMessage);
    }

    // Method to send a MimeMessage via SMTP
    private async Task<bool> SendMailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        try
        {
            // Connect to SMTP server
            await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);

            // Remove XOAUTH2 authentication mechanism
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            // Authenticate with username and password
            await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);

            // Send the email
            await client.SendAsync(message);
        }
        catch (Exception e)
        {
            // Log any errors
            logger.LogError(e, e.Message);
            return false;
        }
        finally
        {
            // Disconnect from SMTP server
            await client.DisconnectAsync(true);
        }

        // Email sent successfully
        return true;
    }

    // Method to create a MimeMessage from an EmailMessage
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