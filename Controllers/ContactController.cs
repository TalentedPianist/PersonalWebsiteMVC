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

namespace PersonalWebsiteMVC.Controllers
{
    public class ContactController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }
        public IConfiguration _configuration { get; set; }
        public ILogger<ContactController> _Logger { get; set; }
        private readonly MailSettings _mailSettings;

        public ContactController(ApplicationDbContext db, IHttpContextAccessor http, IConfiguration config, ILogger<ContactController> logger, IOptions<MailSettings> mailSettingsOptions)
        {
            _db = db;
            _http = http;
            _configuration = config;
            _Logger = logger;
            _mailSettings = mailSettingsOptions.Value;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("ContactForm")]
        public IActionResult ContactForm(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                if (ReCaptchaPassed(
                    Request.Form["g-recaptcha-response"]!,
                    _configuration.GetSection("reCaptcha:secret").Value!,
                    _Logger))
                {
                    // Begin send email code
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(model.Name, model.Email));
                    message.To.Add(new MailboxAddress("Douglas McGregor", "douglas@douglasmcgregor.co.uk"));
                    message.Subject = "Contact Form";
                    message.Body = new TextPart("html") { Text = model.Message };

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate("douglasmcgregor09@gmail.com", "uvqf uhyz ocya rbcy");
                        mailClient.Send(message);
                        mailClient.Disconnect(true);
                    }
                }
            }
            // End send email code

            return View("~/Views/Partial/Contact.cshtml");

        }


        public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret, ILogger _Logger)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _Logger.LogError("Erorr while sending request to ReCaptcha");
                return false;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }
            return true;
        }
    }
}