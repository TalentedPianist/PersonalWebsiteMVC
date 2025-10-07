using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using Wangkanai.Extensions;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
    [Area("pCloud")]
    public class HomeController : Controller
    {
          public IHttpClientFactory _httpClientFactory { get; }

          public HomeController(IHttpClientFactory httpClientFactory)
          {
               _httpClientFactory = httpClientFactory;
          }

          [Route("/pCloud/")]
          public IActionResult Home()
          {
               return View("~/Areas/pCloud/Views/Albums/Index.cshtml");
          }

          [HttpPost]
          [Route("/pCloud/GetThumbs")]
          public async Task<IActionResult> GetThumbs(int getauth, string username, string password)
          {
               var httpClient = _httpClientFactory.CreateClient();
               var response = await httpClient.GetAsync($"https://eapi.pcloud.com?getauth={getauth}&username={username}&password={password}");
               var content = await response.Content.ReadAsStringAsync();
               return Ok(content);

          }
       
    }
}
