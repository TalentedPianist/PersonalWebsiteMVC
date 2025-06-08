namespace PersonalWebsiteMVC.Services;

public interface IEmailService
{
    Task<string> SendEmail(string toEmail, string subject, string body);
}
