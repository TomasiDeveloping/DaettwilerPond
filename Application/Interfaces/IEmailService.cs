using Application.Models;

namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage message);
}