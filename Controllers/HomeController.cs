using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
	public class HomeController : Controller
	{
		

		public IActionResult Index()
		{
			ViewBag.Current = "";	
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
	}
}
