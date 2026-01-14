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
using X.PagedList.Extensions;
using X.PagedList;
using Newtonsoft.Json.Linq;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{

     [Area("pCloud")]
     [Authorize(Roles = "Admin")]
     public class PhotosController : Controller
     {
          public IHttpClientFactory _httpClientFactory { get; set; }
          public IHttpContextAccessor _http { get; set; }
          public ApplicationDbContext _db { get; set; }
          public IWebHostEnvironment _env { get; set; }

          public PhotosController(IHttpClientFactory httpClientFactory, IHttpContextAccessor http, ApplicationDbContext db, IWebHostEnvironment env)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
               _db = db;
               _env = env;
          }

          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/")]
          public IActionResult Index([FromQuery(Name = "id")] string id, [FromQuery(Name="pageNumber")]int? page)
          {
               var client = new RestClient("https://eapi.pcloud.com/listfolder");
               var request = new RestRequest();
               //request.AddParameter("folderid", HttpContext.Request.Query["id"]);
               request.AddParameter("path", $"/Public Folder/Gallery/{HttpContext.Request.Query["name"]}");
               request.AddParameter("getauth", "1");
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               var response = client.Execute(request);
               var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
               StringBuilder sb = new StringBuilder();
               var pageNumber = page ?? 1;

               var model = result!.metadata.contents;

               return View(model);

          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/DeleteFromCloud")]
          public IActionResult DeleteFromCloud(string fileid)
          {
               var client = new RestClient("https://eapi.pcloud.com/deletefile");
               var request = new RestRequest();
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               request.AddParameter("fileid", fileid);
               var response = client.Execute(request);
               return Ok(response.Content);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/GetPopup")]
          public IActionResult GetPopup(ContentItem item, string thumb)
          {
               ViewBag.Image = thumb;
               return PartialView("~/Areas/pCloud/Views/Photos/Popup.cshtml", item);
          }

          public IActionResult Upload()
          {
               return View();
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("pCloud/Photos/UploadFiles")]
          public async Task<IActionResult> UploadFile(IFormFile file, [FromForm(Name="folderid")]string folderid)
          {

               string uploadPath = System.IO.Path.Combine(_env.WebRootPath, "Uploads");
               string filePath = System.IO.Path.Combine(uploadPath, file.FileName);
               using (var stream = new FileStream(filePath, FileMode.Create))
               {
                    await file.CopyToAsync(stream);
                    var client = new RestClient("https://eapi.pcloud.com/uploadfile");
                    var request = new RestRequest();
                    request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
                    request.AddParameter("password", "Inkyfrog1");
                    request.AddParameter("folderid", folderid);
                    request.AddParameter("filename", file.FileName);
                    request.AddFile(file.FileName, filePath);
                    var response = client.Execute(request);
                    TempData["Message"] = response.Content;
               }
                    return View("~/Areas/pCloud/Views/Photos/Upload.cshtml");
          }
          
     }
}
