using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using ServiceLog.Application;
using ServiceLog.Application.Interfaces;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("ServiceLog", _emailSettings.From));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            TextBody = body
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_emailSettings.SmtpServer, int.Parse(_emailSettings.Port), useSsl: false);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
