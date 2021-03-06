using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using PersonalWebsiteMVC.Entities;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Services
{
     public interface IEmailSender
     {
          Task SendEmailAsync(string email, string subject, string message);
     }

     public class EmailSender : IEmailSender
     {
          private readonly EmailSettings _emailSettings; 
          private readonly IWebHostEnvironment _env;

          public EmailSender(
               IOptions<EmailSettings> emailSettings,
               IWebHostEnvironment env)
          {
               _emailSettings = emailSettings.Value;
               _env = env; 
          }

          public async Task SendEmailAsync(string email, string subject, string message)
          {
               var mimeMessage = new MimeMessage();
               mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
               mimeMessage.To.Add(new MailboxAddress("Douglas", email));
               mimeMessage.Subject = subject; 
               mimeMessage.Body = new TextPart("html")
               {
                    Text = message
               };

               using (var client = new SmtpClient())
               {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_emailSettings.MailServer);
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
               }
          }
     }
}
