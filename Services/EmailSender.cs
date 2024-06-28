using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using PersonalWebsiteMVC.Services;
using MailKit.Net.Smtp;
using PersonalWebsiteMVC.Models;
using ServiceStack;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace PersonalWebsiteMVC.Services
{
    public class EmailSender : IEmailSender
    {
        // Used only for ASP.NET Core Identity UI

        private readonly MailSettings _mailSettings;

        public EmailSender(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress("Douglas", "douglasmcgregor09@gmail.com");
                    MailboxAddress emailTo = new MailboxAddress("", email);
                    emailMessage.To.Add(emailTo);
                    emailMessage.From.Add(emailFrom);

                    emailMessage.Subject = subject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = htmlMessage;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    // This is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
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

        async Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendEmailAsync(email, subject, htmlMessage);
        }
    }

   

}
