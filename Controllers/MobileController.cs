
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
    public IActionResult AddComment(string name, string email, string website, string message, string captchaResponse)
    {
        Comments comment = new Comments();
        comment.CommentAuthor = name;
        comment.CommentAuthorEmail = email;
        comment.CommentAuthorUrl = website;
        comment.CommentContent = message;
        comment.CommentAuthorIP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
        return Ok(comment);
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