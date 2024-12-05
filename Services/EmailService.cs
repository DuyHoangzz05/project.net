// Services/EmailService.cs
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    private readonly string _smtpServer = "smtp.example.com";  // Địa chỉ SMTP server
    private readonly string _smtpUsername = "your_email@example.com";  // Tài khoản email
    private readonly string _smtpPassword = "your_password";  // Mật khẩu email

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient(_smtpServer)
        {
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            Port = 587,  // Cổng SMTP
        };

        var mailMessage = new MailMessage(_smtpUsername, email, subject, message);
        await client.SendMailAsync(mailMessage);
    }
}
