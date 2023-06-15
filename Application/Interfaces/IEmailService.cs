using Application.Models;

namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage message);
    Task<bool> SendFishingLicenseBillAsync(int fishingLicenseYear, string userEmail, string mailContent, byte[] qrBill);
}