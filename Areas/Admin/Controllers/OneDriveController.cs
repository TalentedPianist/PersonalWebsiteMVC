using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Helpers;
using Microsoft.Graph;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     public class OneDriveController : Controller
     {

          

          [Route("/Admin/OneDrive")]
          public IActionResult Index(Settings settings)
          {

            
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }

       
         
     }
}
