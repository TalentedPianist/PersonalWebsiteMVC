
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers;

[Controller]
public class MobileController : Controller
{
    [HttpPost]
    [Route("/Blog/AddComment")]
    public async Task<PartialViewResult> AddComment(Comments comment, string captchaResponse)
    {
        TempData["Message"] = await VerifyCaptcha(captchaResponse);
        return PartialView("~/Views/Mobile/CommentForm.cshtml");
    }

    private async Task<bool> VerifyCaptcha(string captchaResponse)
    {
        var secretKey = "6LeCBlUrAAAAACVipFQ2hXQkaRn1i_pFJEZIegge";
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var captchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(jsonResponse);
            return true;
        }
        return false;
    }

}