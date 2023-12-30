using Application.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage message);
    Task<bool> SendFishingLicenseBillAsync(int fishingLicenseYear, string userEmail, string mailContent, byte[] qrBill);
    Task<bool> SendEmailToMembersAsync(List<string> recipientAddresses, string subject, string mailContent, IFormFileCollection attachments);
}