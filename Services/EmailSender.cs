using Microsoft.AspNetCore.Identity.UI.Services;
using PersonalWebsiteMVC.Services;

namespace PersonalWebsiteMVC.Services
{
    public class EmailSender : IEmailSender
    {
        // Used only for ASP.NET Core Identity UI
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }

    public interface IEmailSender
    {
    }
}
