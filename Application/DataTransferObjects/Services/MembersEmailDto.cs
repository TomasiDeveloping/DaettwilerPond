using Microsoft.AspNetCore.Http;

namespace Application.DataTransferObjects.Services;

public class MembersEmailDto
{
    public List<string> ReceiverAddresses { get; set; }
    public string MailContent { get; set; }
    public string Subject { get; set; }
    public IFormFileCollection Attachments { get; set; }
}