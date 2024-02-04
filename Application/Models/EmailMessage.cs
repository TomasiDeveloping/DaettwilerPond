namespace Application.Models;

// Model representing an email message
public class EmailMessage(IEnumerable<string> to, string subject, string content)
{
    // Recipients of the email
    public IEnumerable<string> To { get; set; } = to;

    // Subject of the email
    public string Subject { get; set; } = subject;

    // Subject of the email
    public string Content { get; set; } = content;
}