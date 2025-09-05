using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using PersonalWebsiteMVC.Services;
using MailKit.Net.Smtp;
using System.Linq.Expressions;
using System.Text;
using ServiceStack;
using Newtonsoft.Json;


namespace PersonalWebsiteMVC.Controllers
{
    public class ContactController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }
        private readonly HttpClient _httpClient;
        private EmailService _EmailService { get; set; }

        public ContactController(ApplicationDbContext db, IHttpContextAccessor http, HttpClient httpClient, EmailService emailService)
        {
            _db = db;
            _http = http;
            _httpClient = httpClient;
            _EmailService = emailService;
        }

        [Microsoft.AspNetCore.Mvc.Route("/Contact")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Contact.cshtml");
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("/Contact/SubmitForm")]
        public async Task<IActionResult> ContactForm(string name, string email, string website, string message, string captchaResponse)
        {
            if (await VerifyCaptcha(captchaResponse))
            {
                await _EmailService.SendEmailAsync(email, "Contact Form", message);
                return Ok();
            }
            else
            {
                return Json(new { error = "You must do the captcha to prove that you are human." });
            }
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

    }
}