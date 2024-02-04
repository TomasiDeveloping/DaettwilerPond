namespace Application.Models;

// Model representing the email configuration settings
public class EmailConfiguration
{
    // Sender email address
    public string From { get; set; }

    // SMTP server address
    public string SmtpServer { get; set; }

    // Port number for the SMTP server
    public int Port { get; set; }

    // Username for authentication (if required)
    public string UserName { get; set; }

    // Password for authentication (if required)
    public string Password { get; set; }
}