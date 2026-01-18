using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

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
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("userinfo");
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");

               var response = await client.ExecuteAsync(request);
               Console.WriteLine(response.Content);
               return View();

          }

        
     }
}
