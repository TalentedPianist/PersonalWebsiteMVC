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
using System.Net;
using DeviceDetectorNET.Cache;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Wangkanai.Detection.Services;
using System.Text;

namespace PersonalWebsiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _http;

        private readonly IAntiforgery _antiforgery;
        private readonly IDetectionService _detectionService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IHttpContextAccessor http, IAntiforgery antiforgery, IDetectionService detectionService)
        {
            _logger = logger;
            _db = db;
            _http = http;
            _antiforgery = antiforgery;
            _detectionService = detectionService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            var device = _detectionService.Device.Type;
            if (device.ToString().Contains("Mobile"))
            {
                return View("~/Views/Mobile/Mobile.cshtml");
            }
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



    }
}