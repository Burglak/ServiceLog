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

    public async Task SendEmailAsync(string toEmail, string subject, string bodyPlain, string bodyHtml)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("ServiceLog", _emailSettings.From));
        emailMessage.To.Add(MailboxAddress.Parse(toEmail));
        emailMessage.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = bodyPlain,
            HtmlBody = bodyHtml
        };

        emailMessage.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.SmtpServer, int.Parse(_emailSettings.Port), useSsl: false);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

}
