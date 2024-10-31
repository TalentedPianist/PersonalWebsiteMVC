using Elastic.Clients.Elasticsearch.Snapshot;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
    public class UserController : Controller
    {
        public readonly IConfiguration _configuration;
        public readonly HttpClient _httpClient;

        public UserController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }
    }

    [HttpGet("Captcha")]
    // Thankfully I only have to do this once for the user forms and that's reCaptcha fully integrated!!
    public async Task<bool> GetreCaptchaResponse(string userResponse)
    {
        var reCaptchaSecretKey = _configuration["reCaptchaSecretKey"];
        if (reCaptchaSecretKey != null && userResponse != null)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"secret", reCaptchaSecretKey },
                {"response", userResponse }
            });
            var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<reCaptchaResponse>();
                return result.Success;
            }
        }
    }
}
