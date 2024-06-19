using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Services
{
    public class ReCaptchaFormHttpClient(HttpClient http) : IReCaptchaFormClient
    {
        public HttpClient http { get; } = http;

        public async Task<ReCaptchaFormResult> SendAsync(ReCaptchaModel model)
        {
            var response = await http.PostAsJsonAsync("/send", model);
            if (!response.IsSuccessStatusCode)
            {
                var errorCodes = await response.Content.ReadFromJsonAsync<string[]>() ?? [];
                return ReCaptchaFormResult.Failed("Http Validation failed", errorCodes);
            }
            return ReCaptchaFormResult.Succeeded;
        }
    }
}
