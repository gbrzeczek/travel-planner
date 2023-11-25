using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TravelPlanner.Api.Infrastructure.Email;

public class EmailSender(IOptions<EmailSenderOptions> options) : IEmailSender
{
    private readonly EmailSenderOptions _options = options.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await SendAsync(email, subject, htmlMessage);
    }
    
    private async Task SendAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderAddress));
        message.To.Add(new MailboxAddress(string.Empty, email));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlMessage };

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_options.SenderAddress, _options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}

public class EmailSenderOptions
{
    public required string SenderAddress { get; set; }
    public required string SenderName { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
}