using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Controllers
{
     public class AboutController : Controller
     {
          public IActionResult Index()
          {
               return View("~/Views/Home/About.cshtml");
          }
     }
}
