using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Helpers;
using MyGraphSdk;

namespace PersonalWebsiteMVC.Areas.OneDrive.Controllers
{
     [Area("OneDrive")]
     public class HomeController : Controller
     {
          
          public OneDriveAuth _oneDrive { get; set; }
          public GraphHelper _graphHelper { get; set; }

          public HomeController(OneDriveAuth oneDrive, GraphHelper graphHelper)
          {
               _oneDrive = oneDrive;
               _graphHelper = graphHelper;
          }

          [Route("/OneDrive")]
          public async Task<IActionResult> Index()
          {
               await Task.CompletedTask;
               return View();
          }
     }
}
