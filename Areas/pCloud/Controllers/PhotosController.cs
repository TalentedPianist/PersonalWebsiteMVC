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

          public PhotosController(IHttpClientFactory httpClientFactory, IHttpContextAccessor http, ApplicationDbContext db, IWebHostEnvironment env, IPCloudAuth auth)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
               _db = db;
               _env = env;
               _auth = auth;
          }

          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/")]
          public async Task<IActionResult> Index([FromQuery(Name = "id")] string id, [FromQuery(Name = "pageNumber")] int? page)
          {
               string accessToken = "655SZGJR8uDME26uZ9n760kZPllUcXeMnXXOpi7bGcN7ny92KC4X";


               try
               {
                    var client = new RestClient("https://eapi.pcloud.com/listfolder");
                    var request = new RestRequest();
                    //request.AddParameter("folderid", HttpContext.Request.Query["id"]);
                    request.AddParameter("access_token", accessToken);
                    request.AddParameter("path", $"/Public Folder/Gallery/{HttpContext.Request.Query["name"]}");
                    request.AddHeader("Authorization", $"Bearer {accessToken}");
                    var response = client.Execute(request);
                    var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
                    StringBuilder sb = new StringBuilder();
                    var pageNumber = page ?? 1;

                    var model = result!.metadata!.contents;

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
               request.AddParameter("access_token", token);
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
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               request.AddParameter("fileid", fileid);
               var response = client.Execute(request);
               return Ok(response.Content);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/DeleteMultiple")]
          public async Task<IActionResult> DeleteMultipleFromCloud(List<string> files)
          {
               string accessToken = "655SZGJR8uDME26uZ9n760kZPllUcXeMnXXOpi7bGcN7ny92KC4X";
               
               files.ForEach(f =>
               {
                    Console.WriteLine(f);
                    var client = new RestClient("https://eapi.pcloud.com/");
                    var request = new RestRequest("deletefile");
                    request.AddHeader("Authorization", $"Bearer {accessToken}");
                    request.AddParameter("fileid", f);
                    var response = client.Execute(request);
                    Console.WriteLine(response.Content);
               });
               return Ok(files);
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
          public async Task<IActionResult> UploadFile([FromForm(Name = "files")] List<IFormFile> files)
          {
               string accessToken = "655SZGJR8uDME26uZ9n760kZPllUcXeMnXXOpi7bGcN7ny92KC4X";
               try
               {

                    files.ForEach(async file =>
                    {
                         await UploadToPCloud(accessToken, "/", "/Public Folder/Gallery/Scarborough", file);
                        
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
               await CopyToFolder(token, metadata["path"]!.ToString(), "/Public Folder/Gallery/Scarborough/", metadata["fileid"]!.ToString());
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
     }
}
