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
using reCAPTCHA.AspNetCore.Attributes;


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
        [ValidateRecaptcha(0.5)]
        public async Task<IActionResult> ContactForm(string name, string email, string website, string message, string captchaResponse)
        {
            if (ModelState.IsValid)
            {
                await _EmailService.SendEmailAsync(email, "Contact Form", message);
                return Ok();
            }
               return View("~/Views/Home/Contact.cshtml");
        }

        

    }
}