using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Diagnostics;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;
//using Microsoft.AspNetCore.Components;

namespace PersonalWebsiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _http;

        private readonly IAntiforgery _antiforgery;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IHttpContextAccessor http, IAntiforgery antiforgery)
        {
            _logger = logger;
            _db = db;
            _http = http;
            _antiforgery = antiforgery;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View(_db.Posts);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitContactForm()
        {
            return Ok();
        }

        public static IHtmlContent BuildForm()
        {
            var builder = new HtmlContentBuilder();
            builder.AppendHtml("<div class='commentForm'>");
            builder.AppendHtml("<form method='post'>");
            builder.AppendHtml("<div>");
            builder.AppendHtml("<label for='name'>Name:</label>");
            builder.AppendHtml("<input type='text' name='txtName'>");
            builder.AppendHtml("</div>");
            builder.AppendHtml("<div class='g-recaptcha' data-callback='></div>");
            builder.AppendHtml("<button type='submit'>Submit</button>");
            builder.AppendHtml("</form>");
            builder.AppendHtml("</div>");

            using var writer = new StringWriter();
            builder.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }

        [Route("PhotoCommentForm")]
        public IActionResult GetCommentForm()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            HtmlString html = new HtmlString("<p>Hello World!</p>");
            return Ok(tokens);
        }


        [Route("/photos/AddComment")]
        [HttpPost]
        public IActionResult AddComment([FromForm] List<Models.Comments> data)
        {
            return Ok(data);
        }


    }
}