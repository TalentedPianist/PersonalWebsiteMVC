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
using PersonalWebsiteMVC.Areas.pCloud.Helpers;
using PersonalWebsiteMVC.Models;

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

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("pCloud/Photos/UploadFiles")]
          public async Task<IActionResult> UploadFile([FromForm(Name = "files")] List<IFormFile> files)
          {
               var accessToken = _config["PCloud:Local:AccessToken"];
               try
               {

                    files.ForEach(async file =>
                    {
                         await UploadToPCloud(accessToken!, "/", "/Public Folder/Gallery/Scarborough", file);

                    });

                    return RedirectToAction("~/Areas/pCloud/Views/Photos/Index.cshtml");
               }
               catch (NullReferenceException ex)
               {
                    TempData["Message"] = ex.Message;
                    return View("~/Areas/pCloud/Views/Photos/Index.cshtml");
               }
          }



          public async Task<JToken> UploadToPCloud(string token, string frompath, string topath, IFormFile file)
          {
               Console.WriteLine("Trying to upload to pCloud...");
               Console.WriteLine(token);
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("uploadfile", Method.Post);
               //request.AddParameter("access_token", token);
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("path", topath);
               request.AddParameter("filename", file.FileName);
               request.AddFile("file", () => file.OpenReadStream(), file.FileName, file.ContentType);
               var response = client.ExecuteAsync(request).Result;
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }

               var json = JsonConvert.DeserializeObject(response.Content!);
               JToken result = JToken.Parse(json!.ToString()!);
               var metadata = result["metadata"]![0];
               Console.WriteLine(metadata!["path"]);
               await CopyToFolder(token, metadata["path"]!.ToString(), $"/Public Folder/Gallery/{topath}", metadata["fileid"]!.ToString());
               return metadata!;
          }




          public async Task CopyToFolder(string token, string frompath, string topath, string fileid)
          {

               Console.WriteLine("Trying to copy to folder...");
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("copyfile");
               request.AddHeader("Authorization", $"Bearer {token}");
               //request.AddParameter("access_token", token);
               request.AddParameter("fileid", fileid);
               request.AddParameter("path", frompath);
               request.AddParameter("topath", topath);
               var response = client.ExecuteAsync(request).Result;
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               await DeleteFileFromRoot(token, frompath);


          }

          public async Task DeleteFileFromRoot(string token, string path)
          {
               Console.WriteLine("Trying to delete file....");
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
