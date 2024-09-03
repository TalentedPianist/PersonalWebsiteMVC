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