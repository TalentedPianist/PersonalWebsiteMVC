using Json.More;
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

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     [Area("pCloud")]
     [Authorize(Roles = "Admin")]
     public class HomeController : Controller
     {
          public ApplicationDbContext _db { get; set; }
          public IHttpClientFactory _httpClientFactory { get; }
          public IPCloudAuth _auth { get; set; }

          public HomeController(IHttpClientFactory httpClientFactory, ApplicationDbContext db, IPCloudAuth auth)
          {
               _httpClientFactory = httpClientFactory;
               _db = db;
               _auth = auth;
          }


          public IActionResult Index([FromQuery(Name = "code")] string code)
          {

               var client = new RestClient("https://eapi.pcloud.com/listfolder");
               var request = new RestRequest();

               request.AddParameter("access_token", "655SZGJR8uDME26uZ9n760kZPllUcXeMnXXOpi7bGcN7ny92KC4X");
               request.AddParameter("folderid", "19500076302");
               request.AddParameter("path", $"/Public Folder/Gallery/{HttpContext.Request.Query["name"]}");
               var response = client.Execute(request);
               StringBuilder sb = new StringBuilder();

               //TempData["Message"] = response.Content;
               var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
               List<ContentItem> items = result!.metadata!.contents!;
               return View(items);
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
               try
               {
                    var client = new RestClient("https://eapi.pcloud.com/createfolder");
                    var request = new RestRequest();
                    request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
                    request.AddParameter("password", "Inkyfrog1");
                    request.AddParameter("folderid", folderid);
                    request.AddParameter("name", model.Name);
                    var response = client.Execute(request);
                    TempData["Message"] = folderid;
               }
               catch (NullReferenceException ex)
               {
               }
               return View("~/Areas/pCloud/Views/Home/Create.cshtml");
          }


          [HttpPost]
          public Task GetAuth()
          {
               _auth.Auth();
               return Task.CompletedTask;
          }

     }
}
