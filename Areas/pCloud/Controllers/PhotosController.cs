using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using System.Text;
using SharpCompress;
using PersonalWebsiteMVC.Data;
using Microsoft.AspNetCore.Authorization;
using RestSharp;
using ServiceStack;
using Newtonsoft.Json;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{

     [Area("pCloud")]
     [Authorize(Roles = "Admin")]
     public class PhotosController : Controller
     {
          public IHttpClientFactory _httpClientFactory { get; set; }
          public IHttpContextAccessor _http { get; set; }
          public ApplicationDbContext _db { get; set; }

          public PhotosController(IHttpClientFactory httpClientFactory, IHttpContextAccessor http, ApplicationDbContext db)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
               _db = db;
          }

          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/")]
          public IActionResult Index([FromQuery(Name = "id")] string id)
          {
               var client = new RestClient("https://eapi.pcloud.com/listfolder");
               var request = new RestRequest();
               request.AddParameter("folderid", id);
               request.AddParameter("getauth", "1");
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               var response = client.Execute(request);
               var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
               List<ContentItem> model = result!.metadata.contents;

               return View(model);

          }
     }
}
