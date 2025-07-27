
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



    public async Task<IActionResult> SubmitContactForm(ContactFormModel model)
    {
        TempData["Message"] = model.Name;
        await Task.CompletedTask;
        return View("~/Views/Mobile/Mobile.cshtml");
    }

    // https://www.c-sharpcorner.com/blogs/implementing-captcha-in-asp-net-core-web-application
    [HttpPost]
    [Route("/Blog/AddComment")]
    public async Task<IActionResult> AddComment(MixModel model, [FromForm(Name = "PostTitle")] string title, [FromForm(Name = "PostID")] int id)
    {
        TempData["Message"] = model.Comments.CommentAuthor;


        model.Posts = _db.Posts.Where(p => p.PostTitle == title).FirstOrDefault();
        model.Comments = new Comments();
        model.AllComments = _db.Comments.Where(p => p.PostID == id).ToList();

        // if (!await VerifyCaptcha(captchaResponse))
        // {
        //     return View("~/Views/Mobile/SinglePost.cshtml", model);
        // }
        // else
        // {
        //     ModelState.AddModelError("", "Error validating reCaptcha.  Please try again.");
        //     return View("~/Views/Mobile/SinglePost.cshtml", model);
        // }
        await Task.CompletedTask;
        return View("~/Views/Mobile/SinglePost.cshtml", model);
    }

    // private async Task<bool> VerifyCaptcha(string captchaResponse)
    // {
    //     var secretKey = "6LeCBlUrAAAAACVipFQ2hXQkaRn1i_pFJEZIegge";
    //     var httpClient = new HttpClient();
    //     var response = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}");
    //     if (response.IsSuccessStatusCode)
    //     {
    //         var jsonResponse = await response.Content.ReadAsStringAsync();
    //         var captchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(jsonResponse);
    //         return captchaResult!.Success;

    //     }
    //     return false;
    // }

}