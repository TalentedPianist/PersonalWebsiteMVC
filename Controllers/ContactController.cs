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



namespace PersonalWebsiteMVC.Controllers
{
    public class ContactController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }
        public IConfiguration _configuration { get; set; }
        public ILogger<ContactController> _Logger { get; set; }
        private readonly MailSettings _mailSettings;
  
        private readonly HttpClient _httpClient;

        public ContactController(ApplicationDbContext db, IHttpContextAccessor http, IConfiguration config, ILogger<ContactController> logger, IOptions<MailSettings> mailSettingsOptions, HttpClient httpClient)
        {
            _db = db;
            _http = http;
            _configuration = config;
            _Logger = logger;
            _mailSettings = mailSettingsOptions.Value;
            _httpClient = httpClient;
        }

        [Microsoft.AspNetCore.Mvc.Route("/Contact")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Contact.cshtml");
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("ContactForm")]
        public IActionResult ContactForm(ContactFormModel model)
        {
            if (ModelState.IsValid)
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
                // End send email code 
        

            return View("~/Views/Home/Contact.cshtml");

        }

        [HttpGet("Captcha")]
        public async Task<bool> GetreCaptchaResponse(string userResponse)
        {
            var reCaptchaSecretKey = _configuration["reCaptchaSecretKey"];

            if (reCaptchaSecretKey != null && userResponse != null)
            {
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
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

    }
}