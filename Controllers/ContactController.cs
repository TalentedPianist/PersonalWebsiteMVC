using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using PersonalWebsiteMVC.Services;

namespace PersonalWebsiteMVC.Controllers
{
    public class ContactController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }
        public IConfiguration _configuration { get; set; }
        public ILogger<ContactController> _Logger { get; set; }
        public IMailService _mailService { get; set; } 

        public ContactController(ApplicationDbContext db, IHttpContextAccessor http, IConfiguration config, ILogger<ContactController> logger, IMailService mailService)
        {
            _db = db;
            _http = http;
            _configuration = config;
            _Logger = logger;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("ContactForm")]
        public async Task<IActionResult> ContactForm(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                if (ReCaptchaPassed(
                    Request.Form["g-recaptcha-resonse"]!,
                _configuration.GetSection("reCaptcha:secret").Value!,
                _Logger
                ))
                {
                    await _mailService.SendMailAsync(model.Name!, "douglas@douglasmcgregor.co.uk", "Contact Form", model.Email!);
                    return View("~/Views/Partial/Contact.cshtml");
                }
            }
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