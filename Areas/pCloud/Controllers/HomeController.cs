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


          public IActionResult Index([FromQuery(Name = "code")] string code)
          {
               try
               {
                    var client = new RestClient("https://eapi.pcloud.com/listfolder");
                    var request = new RestRequest();

                    request.AddParameter("access_token", _config["pCloud:Local:AccessToken"]);
                    request.AddParameter("folderid", "19500076302");
                    request.AddParameter("path", $"/Public Folder/Gallery/{HttpContext.Request.Query["name"]}");
                    var response = client.Execute(request);
                    StringBuilder sb = new StringBuilder();

                    //TempData["Message"] = response.Content;
                    var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
                    List<ContentItem> items = result!.metadata!.contents!;
                    return View(items);
               }
               catch (NullReferenceException ex)
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
                    request.AddParameter("access_token", _config["PCloud:Local:AccessToken"]);
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
