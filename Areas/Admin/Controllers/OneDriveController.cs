using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Helpers;
using Microsoft.Graph;


namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     public class OneDriveController : Controller
     {
          private readonly GraphServiceClient _graphServiceClient;

          public OneDriveController(GraphServiceClient graphServiceClient)
          {
               _graphServiceClient = graphServiceClient;
          }

          [Route("/Admin/OneDrive")]
          public IActionResult Index(Settings settings)
          {

               var user = _graphServiceClient.Me.GetAsync();
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }

          [HttpPost]
          public async Task<IActionResult> Login()
          {
             
           
               return View("~/Areas/Admin/Views/OneDrive/Index.cshtml");
          }
         
     }
}
