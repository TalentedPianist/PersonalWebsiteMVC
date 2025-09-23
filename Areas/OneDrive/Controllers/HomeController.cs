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
               string id = "01P4PVLA2IIBAUUOFVSFB2E2MWMDUIORAI";
               var result = await _graphHelper.Graph().Me.Drives.GetAsync();

               TempData["Message"] = result;
               return View();
          }
     }
}
