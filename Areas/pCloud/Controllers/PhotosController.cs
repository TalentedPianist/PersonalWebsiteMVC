using Avalonia.Controls.Shapes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Areas.pCloud.Helpers;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;
using ServiceStack;
using SharpCompress;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using X.PagedList;
using X.PagedList.Extensions;

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
          public IPCloudAuth _auth { get; set; }
          public IConfiguration _config { get; set; }


          public PhotosController(IHttpClientFactory httpClientFactory, IHttpContextAccessor http, ApplicationDbContext db, IWebHostEnvironment env, IPCloudAuth auth, IConfiguration config)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
               _db = db;
               _env = env;
               _auth = auth;
               _config = config;
          }


          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/")]
          public async Task<IActionResult> Index([FromQuery(Name = "name")] string name, [FromQuery(Name = "pageNumber")] int? page)
          {
               try
               {
                    var client = new RestClient("https://eapi.pcloud.com/");
                    var request = new RestRequest("listfolder");
                    request.AddParameter("access_token", Environment.GetEnvironmentVariable("PCloudToken"));
                    request.AddParameter("folderid", HttpContext.Request.Query["id"]);
                    var response = await client.ExecuteAsync(request);
                    if (!response.IsSuccessful)
                    {
                         Console.WriteLine(response.StatusCode);
                         Console.WriteLine(response.ErrorMessage);
                         Console.WriteLine(response.ErrorException);
                         Console.WriteLine(response.Content);
                    }
                    //Console.WriteLine(response.Content);
                    var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
                    List<ContentItem> model = result!.metadata!.contents!;
                    return View(model);
               }
               catch (NullReferenceException)
               {
                    TempData["Message"] = "User needs to login.";
                    return View();
               }
          }

          public async Task<string> ListFolder(string token)
          {
               var client = new RestClient("https://eapi.pcloud.com");
               var request = new RestRequest("listfolder");
               request.AddParameter("access_token", Environment.GetEnvironmentVariable("PCloudToken"));

               request.AddParameter("path", "/");
               var response = await client.ExecuteAsync(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               //Console.WriteLine(response.Content!);
               return response.Content!;
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/DeleteFromCloud")]
          public IActionResult DeleteFromCloud(string fileid)
          {
               var client = new RestClient("https://eapi.pcloud.com/deletefile");
               var request = new RestRequest();
               request.AddParameter("access_token", Environment.GetEnvironmentVariable("PCloudToken"));
               request.AddParameter("fileid", fileid);
               var response = client.Execute(request);
               return Ok(response.Content);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/DeleteMultiple")]
          public async Task<IActionResult> DeleteMultipleFromCloud(List<string> files)
          {

               StringBuilder sb = new StringBuilder();
               foreach (var file in files)
               {
                    Console.WriteLine(file);
                    var client = new RestClient("https://eapi.pcloud.com/");
                    var request = new RestRequest("deletefile");

                    request.AddParameter("access_token", Environment.GetEnvironmentVariable("PCloudToken"));

                    request.AddParameter("fileid", file);

                    var response = await client.ExecuteAsync(request);
                    sb.Append(response.Content);
                    Console.WriteLine(response.Content);
               }
               //TempData["Message"] = sb.ToString();
               return Ok(files);

          }



          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/GetPopup")]
          public IActionResult GetPopup(string item, string thumb)
          {

               ViewBag.Image = thumb;
               JObject json = JObject.Parse(item);
               var model = JsonConvert.DeserializeObject<ContentItem>(json.ToString());
               return PartialView("~/Areas/pCloud/Views/Photos/Popup.cshtml", model);

          }

          public IActionResult Upload()
          {
               return View();
          }

          public async Task<string> FolderID(string name)
          {
               var client = new RestClient("https://eapi.pcloud.com");
               var request = new RestRequest("listfolder");
               request.AddParameter("access_token", Environment.GetEnvironmentVariable("PCloudToken"));

               request.AddParameter("folderid", "19500076302");
               var response = await client.ExecuteAsync(request);
               var json = JsonConvert.DeserializeObject(response.Content!);
               JToken result = JToken.Parse(json!.ToString()!);
               var metadata = result["metadata"];
               return (string)metadata!["folderid"]!.ToString();
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("pCloud/Photos/UploadFiles")]
          public async Task<IActionResult> UploadFile([FromForm(Name = "files")] List<IFormFile> files, [FromForm(Name = "folderid")] string folderid, [FromForm(Name = "name")] string name)
          {


               try
               {
                    foreach (IFormFile file in files)
                    {
                         Console.WriteLine(name);
                         await UploadToPCloud(Environment.GetEnvironmentVariable("PCloudToken")!, string.Empty, $"/", $"/Public Folder/Gallery/{name}", file);

                    }

                    return RedirectToAction("Index", new RouteValueDictionary(new { contorller = "Photos", action = "Index", id = HttpContext.Request.Query["id"], name = name }));
               }
               catch (NullReferenceException ex)
               {
                    TempData["Message"] = ex.Message;
                    return View("~/Areas/pCloud/Views/Photos/Index.cshtml");
               }

          }



          public async Task UploadToPCloud(string token, string name, string frompath, string topath, IFormFile file)
          {
               Console.WriteLine("Trying to upload to pCloud...");
               Console.WriteLine($"Name - {name}");
               Console.WriteLine($"frompath - {frompath}");
               Console.WriteLine($"topath - {topath}");
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("uploadfile", Method.Post);
               request.AddHeader("Authorization", $"Bearer {token}");
               //var finalPath = $"/Public Folder/Gallery/{path}/";
               //var encodedPath = Uri.EscapeDataString(finalPath);

               request.AddParameter("topath", $"/Public Folder/Gallery/{name}");
               request.AddParameter("filename", file.FileName);

               using var ms = new MemoryStream();
               await file.CopyToAsync(ms);
               var fileBytes = ms.ToArray();

               request.AddFile("file", fileBytes, file.FileName, file.ContentType);

               var response = await client.ExecuteAsync(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               //Console.WriteLine(response.Content);
               var json = JsonConvert.DeserializeObject(response.Content!);
               //Console.WriteLine(json);
               JToken result = JToken.Parse(json!.ToString()!);
               var metadata = result["metadata"]![0];
             
               Console.WriteLine(metadata!["fileid"]);
               await Task.CompletedTask;

          }




          public async Task<Task> CopyToFolder(string token, string name, string path, string tofolderid, string fileid)
          {

               Console.WriteLine("Trying to copy to folder...");

               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("copyfile");
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("fileid", fileid);
               request.AddParameter("topath", $"Public Folder/Gallery/{name}");

               var response = await client.ExecuteAsync(request);
               Console.WriteLine(response.Content);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               Console.WriteLine(response.Content);
               await DeleteFileFromRoot(token, path, fileid);
               return Task.CompletedTask;

          }

          public async Task<Task> DeleteFileFromRoot(string token, string path, string fileid)
          {
               Console.WriteLine("Trying to delete file....");
               Console.WriteLine(path);
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("deletefile");
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("fileid", fileid);
               var response = client.ExecuteAsync(request).Result;
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               //Console.WriteLine(response.Content);
               return Task.CompletedTask;
          }



          [HttpPost]
          public Task GetToken()
          {
               _auth.Auth();
               return Task.CompletedTask;
          }

          [HttpGet]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/GetID")]
          public int GetID(string name)
          {
               try
               {
                    var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
                    return photo!.PhotoID;
               }
               catch (NullReferenceException)
               {
                    // Photo is not in the database
                    return 0;
               }
          }

          [HttpGet]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/GetAlbumID")]
          public int GetAlbumID(string name)
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               return album!.AlbumID;
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/AddToDb")]
          public IActionResult AddToDb([FromBody] List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.AddRange(data);
               _db.SaveChanges();
               TempData["Message"] = "Photos added to database.";
               return Ok(data);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/DelFromDb")]
          public IActionResult DelFromDb([FromBody] List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.RemoveRange(data);
               _db.SaveChanges();
               TempData["Message"] = "Photos deleted from database.";
               return Ok(data);
          }


          public IActionResult GetThumb(string fileid)
          {
               var bytes = GetPubLink(fileid, "600x400");
               return File(bytes, "image/jpeg");
          }

          byte[] GetPubLink(string fileid, string size)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("getthumb", Method.Get);
               request.AddParameter("access_token", Environment.GetEnvironmentVariable("pCloudToken"));
               request.AddParameter("fileid", fileid);
               request.AddParameter("size", size);
               request.AddParameter("type", "jpeg");
               var response = client.ExecuteAsync(request).Result;
               return response.RawBytes!;
          }

     }
}
