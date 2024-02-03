using Microsoft.AspNetCore.Http;

namespace Application.DataTransferObjects.Services
{
    // Data Transfer Object (DTO) representing email-related information for sending to multiple members
    public class MembersEmailDto
    {
        // List of email addresses of the receivers
        public List<string> ReceiverAddresses { get; set; }

        // Content of the email
        public string MailContent { get; set; }

        // Subject of the email
        public string Subject { get; set; }

        // Collection of file attachments for the email (if any)
        public IFormFileCollection Attachments { get; set; }
    }
}