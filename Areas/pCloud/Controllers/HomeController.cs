using Json.More;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;
using ServiceStack;
using SharpCompress;
using System.Text;
using Wangkanai.Extensions;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     [Area("pCloud")]
     [Authorize(Roles = "Admin")]
     public class HomeController : Controller
     {
          public ApplicationDbContext _db { get; set; }
          public IHttpClientFactory _httpClientFactory { get; }

          public HomeController(IHttpClientFactory httpClientFactory, ApplicationDbContext db)
          {
               _httpClientFactory = httpClientFactory;
               _db = db;
          }


          public IActionResult Index([FromQuery(Name = "code")] string code)
          {
               string AccessToken = "655SZGJR8uDME26uZdTpK0kZAEJfaNqdDmfMc5Aku6CMUuK1q7Ay";

               string clientId = "GJR8uDME26u";

               string url = $"https://my.pcloud.com/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri=http://localhost:5051/pCloud/";
               //TempData["Message"] = url;

               var client = new RestClient("https://eapi.pcloud.com/listfolder");
               var request = new RestRequest();
               request.AddParameter("folderid", "19500076302");
               request.AddParameter("getauth", "1");
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               var response = client.Execute(request);
               StringBuilder sb = new StringBuilder();
             
               try
               {
                    var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
                    var folder = result!.metadata;
                    List<ContentItem> items = result.metadata.contents;
                    return View(items);
               }
               catch (NullReferenceException)
               {
               }
               return View();

          }


          [HttpPost]
          public IActionResult Auth()
          {
               string clientId = "GJR8uDME26u";

               string url = $"https://my.pcloud.com/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri=http://localhost:5051/pCloud/&state=12345&force_reapprove=true";
               return Redirect(url);
          }

          string GetAccessToken()
          {

               string clientId = "GJR8uDME26u";
               string clientSecret = "U83OQca6ABpaiDtaBsStUbgKRiAk";
               string url = "https://eapi.pcloud.com/oauth2_token";

               var client = new RestClient(url);
               var request = new RestRequest();
               request.AddParameter("client_id", clientId);
               request.AddParameter("client_secret", clientSecret);
               request.AddParameter("code", HttpContext.Request.Query["code"]);
               request.AddParameter("access_token", "655SZGJR8uDME26uZdTpK0kZAEJfaNqdDmfMc5Aku6CMUuK1q7Ay");
               
               var response = client.Execute(request);
               var json = JsonConvert.DeserializeObject<pCloudToken>(response.Content!);

               return json!.access_token!;

          }




     }
}
