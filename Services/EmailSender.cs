namespace MembersUmbraco.Services;

using System.Runtime.CompilerServices;

// 10-06-2025: Using the Email send functionality here in this file
// using Microsoft.AspNetCore.Identity.UI.Services;

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

// 10-06-2025: Using the AppSettings.cs for the Email send functionality here in this file
using MembersUmbraco.Helpers;

public interface IEmailSender
{
     Task SendEmailAsync(string to, string subject, string html, string from = null);
}

public class EmailSender : IEmailSender
{
    private readonly AppSettings _appSettings;

    public EmailSender(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string html, string from = null)
    {
        if (string.IsNullOrEmpty(to))
        {
            throw new Exception("Null toEmail");
        }
        await Execute(to, subject, html, from);
    }

    public async Task Execute(string to, string subject, string html, string from = null)
    {
        // create message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        // send email
        using var smtp = new SmtpClient();
        smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
        
        // Send Email Asyncron
        await smtp.SendAsync(email);

        smtp.Disconnect(true);
    }


}