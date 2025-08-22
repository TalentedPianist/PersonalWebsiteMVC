
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Services;

namespace PersonalWebsiteMVC.Controllers;


[Controller]
public class MobileController : Controller
{
    public ApplicationDbContext _db { get; set; }
    public IHttpContextAccessor _http { get; set; }
    public EmailService _emailService { get; set; }

    public MobileController(ApplicationDbContext db, IHttpContextAccessor http, EmailService emailService)
    {
        _db = db;
        _http = http;
        _emailService = emailService;
    }


    // https://www.c-sharpcorner.com/blogs/implementing-captcha-in-asp-net-core-web-application
    [HttpPost]
    [Route("/Blog/AddComment")]
    public async Task<IActionResult> AddComment(MixModel model, [FromForm(Name = "PostTitle")] string title, [FromForm(Name = "PostID")] int id)
    {
        TempData["Message"] = model.Comments.CommentAuthor;


        model.Posts = _db.Posts.Where(p => p.PostTitle == title).FirstOrDefault();
        model.AllComments = _db.Comments.Where(p => p.PostID == id).ToList();

        model.Comments.CommentAuthorIP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
        model.Comments.CommentDate = DateTime.Now;
        _db.Comments.Add(model.Comments);
        _db.SaveChanges();
        return RedirectToAction(title, "Blog");

        // ModelState.AddModelError("", "Error validating reCaptcha.  Please try again.");
        // return View("~/Views/Mobile/SinglePost.cshtml", model);


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
            return captchaResult!.Success;

        }
        return false;
    }

    [HttpPost]
    [Route("/Mobile/SubmitContactForm")]
    public async Task<IActionResult> SubmitContactForm(string name, string email, string website, string message, string captchaResponse)
    {
        if (await VerifyCaptcha(captchaResponse))
        {
            // Captcha verification successful, send email
            await _emailService.SendEmailAsync(email, "Contact Form", message);
            return Ok(captchaResponse);
        }
        else
        {
            return Json("Captcha verification failed, please try again.");
        }
    }

}