using Microsoft.Extensions.Options;
using MimeKit;
using PersonalWebsiteMVC.Models;
using MailKit.Net.Smtp;

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
        public async Task<bool> SendMailAsync(string email, string name, string body)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    MailboxAddress emailTo = new MailboxAddress(name, email);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = "Contact Form";

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
                Console.WriteLine(ex.Message + " - " + ex.Source);
                return false;
            }
        }

       
    }

    public interface IMailService
    {
        Task<bool> SendMailAsync(string email, string name, string body);
    }
}
