using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Helpers;
using Microsoft.Graph;
using MyGraphSdk;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     public class OneDriveController : Controller
     {

          

          [Route("/Admin/OneDrive")]
          public IActionResult Index(Settings settings)
          {

               var credential = new DeviceCodeCredential();
               var graphClient = new GraphServiceClient(credential);
               var drive = graphClient.Me.Drives.GetAsync().Result;
               StringBuilder sb = new StringBuilder();
               foreach (var item in drive)
               {
                    sb.Append(item);
               }
               TempData["Message"] = sb.ToString();
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }

          [HttpPost]
          public async Task<IActionResult> Login()
          {
             
           
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }
         
     }
}
