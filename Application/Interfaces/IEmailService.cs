using Application.Models;

namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage message);
    Task<bool> SendFishingLicenseBill(int fishingLicenseYear, string userEmail, string mailContent, byte[] qrBill);
}