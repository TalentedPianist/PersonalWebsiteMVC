using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
