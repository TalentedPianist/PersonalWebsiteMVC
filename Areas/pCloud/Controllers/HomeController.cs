using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using PersonalWebsiteMVC.Areas.pCloud.Helpers;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;
using ServiceStack;
using SharpCompress;
using System.Text;
using Wangkanai.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     [Area("pCloud")]
     [Authorize(Roles = "Admin")]
     public class HomeController : Controller
     {
          public ApplicationDbContext _db { get; set; }
          public IHttpClientFactory _httpClientFactory { get; }
          public IPCloudAuth _auth { get; set; }
          public IConfiguration _config { get; set; }
          public IWebHostEnvironment _env { get; set; }

          public HomeController(IHttpClientFactory httpClientFactory, ApplicationDbContext db, IPCloudAuth auth, IConfiguration config, IWebHostEnvironment env)
          {
               _httpClientFactory = httpClientFactory;
               _db = db;
               _auth = auth;
               _config = config;
               _env = env;
          }


          public IActionResult Index([FromQuery(Name="code")]string code)
          {
               if (!string.IsNullOrEmpty(code))
               {
                    var token = _auth.GetAccessToken();
                    HttpContext.Session.SetString("PCloudToken", token);
                    ViewBag.AccessToken = token;
                    return RedirectToAction("Index", new { Area = "PCloud", Controller = "Home" });
               }
       

               try
               {
                    var client = new RestClient("https://eapi.pcloud.com/");
                    var request = new RestRequest("listfolder");


                    request.AddParameter("access_token", HttpContext.Request.Cookies["PCloudToken"]);

                    request.AddParameter("folderid", "19500076302");
                    var response = client.Execute(request);

                    if (!response.IsSuccessful)
                    {
                         Console.WriteLine(response.StatusCode);
                         Console.WriteLine(response.ErrorMessage);
                         Console.WriteLine(response.ErrorException);
                         Console.WriteLine(response.Content);
                    }
                    Console.WriteLine(response.Content);

                    var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
                    List<ContentItem> model = result!.metadata!.contents!;
                    return View(model);
               }
               catch (NullReferenceException)
               {


                    return View();
               }

          }

          public IActionResult Create(string id)
          {
               ViewBag.FolderID = id;
               return View();
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Home/CreateFolder")]
          public IActionResult CreateFolder(CreateFolderModel model, [FromForm(Name = "folderid")] string folderid)
          {

               var client = new RestClient("https://eapi.pcloud.com/createfolder");
               var request = new RestRequest();

               request.AddParameter("access_token", HttpContext.Session.GetString("PCloudToken"));

               request.AddParameter("folderid", folderid);
               request.AddParameter("name", model.Name);
               var response = client.Execute(request);
               TempData["Message"] = folderid;

               return RedirectToAction("Index", new { Area = "pCloud", Controller = "Home" });
          }


          [HttpPost]
          public Task GetAuth()
          {
               _auth.Auth();
               return Task.CompletedTask;
          }

     }
}
