using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;

namespace questvault.Services
{
  public class EmailSender(string host, int port, bool enableSSL, string userName, string password) : IEmailSender
  {
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("QuestVault", "quest.vault@outlook.com"));
      message.To.Add(new MailboxAddress(userName, email));
      message.Subject = subject;
      message.Body = new TextPart(TextFormat.Html)
      {
        Text = htmlMessage
      };

      using var client = new SmtpClient();
      // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
      client.ServerCertificateValidationCallback = (s, c, h, e) => true;

      await client.ConnectAsync(host, port, enableSSL);

      // Note: only needed if the SMTP server requires authentication
      await client.AuthenticateAsync(userName, password);

      await client.SendAsync(message);
      await client.DisconnectAsync(true);
    }

  }
}
