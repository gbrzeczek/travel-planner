using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using TravelPlanner.Api.Entities;

namespace TravelPlanner.Api.Infrastructure.Email;

public class EmailSender(IOptions<EmailSenderOptions> options) : IEmailSender<User>
{
    private readonly EmailSenderOptions _options = options.Value;
    
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        const string subject = "Confirm your email";
        var htmlMessage = $"<a href=\"{confirmationLink}\">Confirm your email</a>";
        return SendAsync(email, subject, htmlMessage);
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        const string subject = "Reset your password";
        var htmlMessage = $"<a href=\"{resetLink}\">Reset your password</a>";
        return SendAsync(email, subject, htmlMessage);
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        const string subject = "Reset your password";
        var htmlMessage = $"<p>Your reset code is: {resetCode}</p>";
        return SendAsync(email, subject, htmlMessage);
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