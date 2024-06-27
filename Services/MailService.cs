using Microsoft.Extensions.Options;
using MimeKit;
using PersonalWebsiteMVC.Models;
using MailKit.Net.Smtp;
using System.Linq.Expressions;

namespace PersonalWebsiteMVC.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        // https://mailtrap.io/blog/asp-net-core-send-email/
        public async Task<bool> SendMailAsync(string name, string email, string subject, string body)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(name, email);
                    MailboxAddress emailTo = new MailboxAddress(name, email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.From.Add(emailFrom);

                    emailMessage.Subject = subject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = body;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    // this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one

                    using (SmtpClient mailClient = new SmtpClient())
                    {

                        await mailClient.ConnectAsync(_mailSettings.Server, Convert.ToInt32(_mailSettings.Port), MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        
    }
    public interface IMailService
    {
        Task<bool> SendMailAsync(string name, string email, string subject, string body);
    }
}


