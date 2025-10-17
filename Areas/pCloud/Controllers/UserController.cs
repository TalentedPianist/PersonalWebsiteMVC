using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     [Area("pCloud")]
     [Authorize(Roles="Admin")]
     public class UserController : Controller
     {
          private IHttpClientFactory _httpClientFactory { get; set; }

          public UserController(IHttpClientFactory httpClientFactory)
          {
               _httpClientFactory = httpClientFactory;
          }

          public async Task<IActionResult> Index()
          {

               var httpClient = _httpClientFactory.CreateClient();

               // First we have to authenticate with pCloud.
               string username = "douglas@douglasmcgregor.co.uk";
               string password = "Inkyfrog1";

               //string myPicturesId = "d12708566907";
               string myPicturesPath = "/My Pictures";
               string folderId = "12708566907";
               var folders = await httpClient.GetStringAsync($"https://eapi.pcloud.com/listfolder?getauth=1&username={username}&password={password}&id={folderId}&path={myPicturesPath}");

               TempData["Message"] = folders;
               return View();

          }

        
     }
}
