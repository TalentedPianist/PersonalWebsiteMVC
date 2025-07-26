
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Models;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Controllers;


[Controller]
public class MobileController : Controller
{
    public ApplicationDbContext _db { get; set; }

    public MobileController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("/Blog/AddComment")]
    public async Task<IActionResult> AddComment(Comments comment, [FromForm(Name="g-recaptcha-response")]string captchaResponse, [FromForm(Name="PostTitle")] string title, [FromForm(Name="PostID")]int id)
    {
        TempData["Message"] = await VerifyCaptcha(captchaResponse);

        MixModel model = new MixModel();
        model.Posts = _db.Posts.Where(p => p.PostTitle == title).FirstOrDefault();
        model.AllComments = _db.Comments.Where(p => p.PostID == id).ToList();

        await Task.CompletedTask;
        return View("~/Views/Mobile/SinglePost.cshtml", model);
    }

    private async Task<string> VerifyCaptcha(string captchaResponse)
    {
        var secretKey = "6LeCBlUrAAAAACVipFQ2hXQkaRn1i_pFJEZIegge";
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var captchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(jsonResponse);
            return jsonResponse;
        }
        return string.Empty;
    }

}