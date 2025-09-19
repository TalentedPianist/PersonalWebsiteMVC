using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using MyGraphSdk;
using PersonalWebsiteMVC.Helpers;


namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     public class OneDriveController : Controller
     {
       

          [Route("/Admin/OneDrive")]
          public IActionResult Index(Settings settings)
          {
          

               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }

          [HttpPost]
          public IActionResult Login()
          {
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }
         
     }
}
