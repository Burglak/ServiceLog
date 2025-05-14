namespace ServiceLog.Application.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string bodyPlain, string bodyHtml);
    }
}
