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
          public static string ClientID = "4cb4c3f8-b663-44bb-9f7c-ec3ee7e42edb";
          public static string ClientSecret = "VhF8Q~YaJrHk39hrm6Xpe03~D3zAiHwHTV1sBbBd";
          public static string TenantID = "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab";
          public static string? AccessToken { get; set; }
          public string RedirectUri = "http://localhost:5051/oneDrive/";

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
               await Task.CompletedTask;
               return View();
          }

          [HttpPost]
          [Route("/OneDrive/Home/Login")]
          public IActionResult Login()
          {
               var scope = new string[] { "User.Read" };
               string url = $"https://login.microsoftonline.com/{TenantID}/oauth2/v2.0/authorize?client_id={ClientID}&response_type=code&redirect_uri={RedirectUri}&response_mode=query&scope={scope}&state=12345";
               return Redirect(url);
          }
     }
}
