using Json.More;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;
using ServiceStack;
using System.Text;
using Wangkanai.Extensions;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
    [Area("pCloud")]
    [Authorize(Roles="Admin")]
    public class HomeController : Controller
    {
          public ApplicationDbContext _db { get; set; }
          public IHttpClientFactory _httpClientFactory { get; }

          public HomeController(IHttpClientFactory httpClientFactory, ApplicationDbContext db)
          {
               _httpClientFactory = httpClientFactory;
               _db = db; 
          }

          public IActionResult Index()
          {
               var client = new RestClient("https://eapi.pcloud.com/listfolder");
               client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", GetAccessToken()));
               var request = new RestRequest();
               request.AddParameter("path", "/");
               var response = client.Execute(request);
               var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
               StringBuilder sb = new StringBuilder();
               List<Contents> model = result!.metadata!.Contents!;
              
               return View(model);
          }


          [HttpPost]
          public IActionResult Auth()
          {
               string clientId = "GJR8uDME26u";

               string url = $"https://my.pcloud.com/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri=http://localhost:5051/pCloud/";
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
               var response = client.Execute(request);
               var json = JsonConvert.DeserializeObject<pCloudToken>(response.Content!);
               return json!.access_token!;
          }

         
              

     }
}
