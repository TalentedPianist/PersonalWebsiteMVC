using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Helpers;
using Microsoft.Graph;
using MyGraphSdk;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     public class OneDriveController : Controller
     {

          

          [Route("/Admin/OneDrive")]
          public IActionResult Index(Settings settings)
          {

               var credential = new DeviceCodeCredential();
               var graphClient = new GraphServiceClient(credential);
               var me = graphClient.Me;
               TempData["Message"] = me.GetAsync().Result!.DisplayName;
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }

          [HttpPost]
          public async Task<IActionResult> Login()
          {
             
           
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }
         
     }
}
