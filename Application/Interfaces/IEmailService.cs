using Application.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

// Interface for email-related operations
public interface IEmailService
{
    // Send a general email message
    Task<bool> SendEmailAsync(EmailMessage message);

    // Send a fishing license bill with specific details
    Task<bool> SendFishingLicenseBillAsync(int fishingLicenseYear, string userEmail, string mailContent, byte[] qrBill);

    // Send an email to multiple recipients with attachments
    Task<bool> SendEmailToMembersAsync(List<string> recipientAddresses, string subject, string mailContent, IFormFileCollection attachments);
}