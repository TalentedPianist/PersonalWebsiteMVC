using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Services
{
    public interface IReCaptchaFormClient
    {
        Task<ReCaptchaFormResult> SendAsync(ReCaptchaModel model);
    }
}
