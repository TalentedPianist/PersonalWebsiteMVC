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
          public async Task<IActionResult> Index([FromQuery(Name = "id")] string id, [FromQuery(Name = "pageNumber")] int? page)
          {
               string accessToken = "655SZGJR8uDME26uZ9n760kZPllUcXeMnXXOpi7bGcN7ny92KC4X";

             

               try
               {
                    var client = new RestClient("https://eapi.pcloud.com/listfolder");
                    var request = new RestRequest();
                    request.AddParameter("access_token", _config["PCloud:Local:AccessToken"]);
                    request.AddParameter("path", $"/Public Folder/Gallery/{HttpContext.Request.Query["name"]}");
                    request.AddHeader("Authorization", $"Bearer {accessToken}");
                    var response = client.Execute(request);
                    var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
                    StringBuilder sb = new StringBuilder();
                    var pageNumber = page ?? 1;

                    var model = result!.metadata!.contents!.ToPagedList(pageNumber, 100);

                    return View(model);
               }
               catch (NullReferenceException ex)
               {
                    TempData["Message"] = ex.Message;
                    return View();
               }

          }

          public async Task<string> ListFolder(string token)
          {
               var client = new RestClient("https://eapi.pcloud.com");
               var request = new RestRequest("listfolder");
               request.AddParameter("access_token", _config["PCloud:Local:AccessToken"]);
               request.AddParameter("path", "/");
               var response = await client.ExecuteAsync(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               Console.WriteLine(response.Content!);
               return response.Content!;
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/DeleteFromCloud")]
          public IActionResult DeleteFromCloud(string fileid)
          {
               var client = new RestClient("https://eapi.pcloud.com/deletefile");
               var request = new RestRequest();
               request.AddParameter("access_token", _config["PCloud:Local:AccessToken"]);
               request.AddParameter("fileid", fileid);
               var response = client.Execute(request);
               return Ok(response.Content);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/DeleteMultiple")]
          public async Task<IActionResult> DeleteMultipleFromCloud(List<string> files)
          {

               StringBuilder sb = new StringBuilder();
               foreach (var fileid in files)
               {
                    var client = new RestClient("https://eapi.pcloud.com");
                    var request = new RestRequest("deletefile");
                    request.AddParameter("access_token", _config["PCloud:Local:AccessToken"]);


                    request.AddParameter("fileid", fileid);

                    var response = await client.ExecuteAsync(request);
                    sb.Append(response.Content);
               }
               TempData["Message"] = sb.ToString();
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
               request.AddParameter("access_token", _config["PCloud:Local:AccessToken"]);
               request.AddParameter("folderid", "19500076302");
               var response = await client.ExecuteAsync(request);
               var json = JsonConvert.DeserializeObject(response.Content!);
               JToken result = JToken.Parse(json!.ToString()!);
               var metadata = result["metadata"];
               return (string)metadata!["folderid"]!.ToString();
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("pCloud/Photos/UploadFiles")]
          public async Task<IActionResult> UploadFile([FromForm(Name = "files")] List<IFormFile> files, [FromForm(Name = "path")] string path)
          {
        
               var accessToken = _config["PCloud:Local:AccessToken"];
     
               foreach (IFormFile file in files)
               {
                    await UploadToPCloud(accessToken!, path, file);
               }

               //return RedirectToAction("~/Areas/pCloud/Views/Photos/Index.cshtml");
               TempData["Message"] = path;
               return View("~/Areas/pCloud/Views/Photos/Index.cshtml");
          }



          public async Task UploadToPCloud(string token, string path, IFormFile file)
          {
               Console.WriteLine("Trying to upload to pCloud...");
               Console.WriteLine(path);
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("uploadfile", Method.Post);
               request.AddHeader("Authorization", $"Bearer {token}");
               var finalPath = $"/Public Folder/Gallery/{path}/";
               var encodedPath = Uri.EscapeDataString(finalPath);
               Console.WriteLine(encodedPath);
               request.AddParameter("path", finalPath);
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
               Console.WriteLine(response.Content);
               var json = JsonConvert.DeserializeObject(response.Content!);
               //Console.WriteLine(json);
               JToken result = JToken.Parse(json!.ToString()!);
               var metadata = result["metadata"]![0];
               //Console.WriteLine(metadata!["path"]);
               //await CopyToFolder(token, metadata!["path"]!.ToString(), path);
               
          }




          public async Task<Task> CopyToFolder(string token, string path, string topath)
          {

               Console.WriteLine("Trying to copy to folder...");
               
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("copyfile");
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("path", path);
               request.AddParameter("topath", $"/Public Folder/Gallery/{topath}/");
               var response = await client.ExecuteAsync(request);
               Console.WriteLine(response.Content);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               //await DeleteFileFromRoot(token, path);
               return Task.CompletedTask;

          }

          public async Task<Task> DeleteFileFromRoot(string token, string path)
          {
               Console.WriteLine("Trying to delete file....");
               Console.WriteLine(path);
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("deletefile");
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("path", path);
               var response = client.ExecuteAsync(request).Result;
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               Console.WriteLine(response.Content);
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
               return Ok(data);
          }

          [HttpPost]
          public IActionResult DelFromDb([FromBody] List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.RemoveRange(data);
               _db.SaveChanges();
               return Ok(data);
          }


          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/GetThumbs")]
          public string GetThumbs(string fileid, string path, string size)
          {
               string accessToken = "655SZGJR8uDME26uZ9n760kZPllUcXeMnXXOpi7bGcN7ny92KC4X";
               var client = new RestClient("https://eapi.pcloud.com/getthumblink");
               var request = new RestRequest();
               request.AddParameter("access_token", accessToken);
               request.AddParameter("path", path);
               request.AddParameter("size", size);
               var response = client.Execute(request);
               var result = JsonConvert.DeserializeObject(response.Content!);
               StringBuilder sb = new StringBuilder();
               JToken json = JToken.Parse(result!.ToString()!);
               var url = "https://" + json["hosts"]![0] + json["path"];
               return url;
          }

     }
}
