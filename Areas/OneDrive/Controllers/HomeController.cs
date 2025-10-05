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
          private IConfiguration settings;
          public IHttpClientFactory _httpClient { get; set; }

          public HomeController(OneDriveAuth oneDrive, GraphHelper graphHelper, IConfiguration settings, IHttpClientFactory httpClient)
          {
               _oneDrive = oneDrive;
               _graphHelper = graphHelper;
               this.settings = settings;
               _httpClient = httpClient;
          }

          [Route("/OneDrive")]
          public async Task<IActionResult> Index()
          {
               TempData["Message"] = await _oneDrive.GetAccessToken();
               await Task.CompletedTask;
               return View();
          }
     }
}
