using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Diagnostics;
using PersonalWebsiteMVC.Components;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Components;

namespace PersonalWebsiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _http;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IHttpContextAccessor http)
        {
            _logger = logger;
            _db = db;
            _http = http;

        }

        [Microsoft.AspNetCore.Mvc.Route("/")]
        [Microsoft.AspNetCore.Mvc.Route("/{Id:int}")]
        [HttpGet]
        public IActionResult Index([FromQuery(Name="id")]int? id)
        {
            // Begin DeviceDetector.NET code
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = Request.Headers["User-Agent"];
            var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);

            var dd = new DeviceDetector(userAgent, clientHints);
            dd.Parse();

            if (dd.IsBot())
            {
                return Content("Go away bot!");
            }
            else
            {
                var clientInfo = dd.GetClient();
                var osInfo = dd.GetOs();
                var device = dd.GetDeviceName();
                var brand = dd.GetBrandName();
                var model = dd.GetModel();

                // https://stackoverflow.com/questions/39870298/how-do-i-specify-different-layouts-in-the-asp-net-core-mvc
                
            }

            // End DeviceDetector.NET code
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


        
    }
}