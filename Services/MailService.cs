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
        public async Task<bool> SendMailAsync(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

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
        Task<bool> SendMailAsync(MailData mailData);
    }
}
