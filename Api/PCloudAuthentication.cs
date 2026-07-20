using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;
using Microsoft.EntityFrameworkCore;
using SharpCompress;
using System.Text;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class PCloudAuthentication : ControllerBase
     {

          public ApplicationDbContext _db { get; set; }
          public IHttpContextAccessor _http { get; set; }
          public string Message { get; set; } = string.Empty;
          public List<string> FileID { get; set; } = new List<string>();

          public PCloudAuthentication(ApplicationDbContext db, IHttpContextAccessor http)
          {
               _db = db;
               _http = http;
          }


          // Correct useage of parameters - string? clientId - bypasses validation errors
          [HttpPost("/api/GetToken")]
          public IActionResult GetToken([FromQuery(Name = "client_id")] string? clientId, [FromQuery(Name = "secret")] string? clientSecret, [FromQuery(Name = "code")] string? code)
          {
               var url = "https://eapi.pcloud.com/";
               var client = new RestClient(url);
               var request = new RestRequest("oauth2_token");
               request.AddParameter("client_id", "GJR8uDME26u");
               request.AddParameter("client_secret", "U83OQca6ABpaiDtaBsStUbgKRiAk");
               request.AddParameter("code", code);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               var json = JsonConvert.DeserializeObject<pCloudToken>(response.Content!);
               var token = json?.access_token == null ? "NULL" : json.access_token;
               return Ok(token);
          }



          [HttpGet("/api/pCloud/ListFolder")]
          public IActionResult ListFolder([FromQuery(Name = "access_token")] string? token, [FromQuery(Name = "folderid")] string? folderid)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("listfolder");
               request.AddParameter("access_token", token);
               request.AddParameter("folderid", folderid);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               var result = JsonConvert.DeserializeObject<PCloudResponse>(response.Content!);
               return Ok(response);
          }

          [HttpGet("/api/pCloud/ListAlbum")]
          public IActionResult ListAlbum([FromQuery(Name = "token")] string? token, [FromQuery(Name = "id")] string? id)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("listfolder");
               request.AddParameter("folderid", id);
               request.AddParameter("access_token", token);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }

               return Ok(response.Content);
          }


          [HttpGet("/api/pCloud/GetThumbLink")]
          public IActionResult GetThumbLink(string? fileid, string? size, [FromQuery(Name = "token")] string? token)
          {

               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("getthumblink");
               request.AddParameter("fileid", fileid);
               request.AddParameter("size", "150x130");
               request.AddParameter("access_token", token);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }

               return Ok(response.Content);
          }

          [HttpGet("/api/pCloud/GetStat")]
          public IActionResult GetPicStats([FromQuery(Name = "fileid")] string fileid, [FromQuery(Name = "token")] string token)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("stat");
               request.AddParameter("fileid", fileid);
               request.AddParameter("access_token", token);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }

               return Ok(response.Content);

          }


          [HttpPut("/api/pCloud/AddAlbumID")]
          public IActionResult AddAlbumID([FromQuery(Name = "name")] string? name, [FromQuery(Name = "id")] string? id)
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               album!.PCloudFolderID = id;
               _db.Albums.Update(album);
               _db.SaveChanges();
               return Ok(album);

          }

          [HttpGet("/api/pCloud/GetAlbumID")]
          public IActionResult GetAlbumID([FromQuery(Name = "name")] string? name)
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               if (album is not null)
               {
                    if (album.PCloudFolderID is not null)
                    {
                         return Ok(album.PCloudFolderID);
                    }
               }
               return NotFound("Album not in database");
          }

          [HttpGet("/api/pCloud/PhotoPicker")]
          public IActionResult PhotoPicker(string? token, string? id)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("listfolder");
               request.AddParameter("access_token", token);
               request.AddParameter("folderid", id);
               var response = client.Execute(request);
               return Ok(response.Content);


          }


          [HttpPost("/api/pCloud/CreateAlbum")]
          public IActionResult CreateAlbum(string? token, string? name)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("createfolderifnotexists");
               request.AddParameter("folderid", "19500076302");
               request.AddParameter("name", name);
               request.AddParameter("access_token", token);
               var response = client.Execute(request);
               return Ok(response.Content);
          }

          [HttpPost("/api/pCloud/UploadFiles")]
          public async Task<IActionResult> UploadFiles([FromForm(Name = "photos")] IFormFile[]? files, [FromQuery(Name = "token")] string? token, [FromQuery(Name = "folderid")] string? folderid, [FromQuery(Name = "foldername")] string? foldername, [FromQuery(Name = "rootFolder")] string? rootFolder)
          {
               var result = string.Empty;
               StringBuilder sb = new StringBuilder();
               foreach (IFormFile file in files!)
               {
                    result = await UploadToPCloud(folderid, token, file, foldername, rootFolder);
                    FileID.Add(result);
               }

               return Ok(FileID);

          }

          public async Task<string> UploadToPCloud(string? folderid, string? token, IFormFile? file, string? foldername, string? rootFolder)
          {

               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("uploadfile", Method.Post);
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("folderid", folderid);
               request.AddParameter("filename", file.Name);

               using var ms = new MemoryStream();
               await file.CopyToAsync(ms);
               var fileBytes = ms.ToArray();

               request.AddFile("file", fileBytes, file.FileName, file.ContentType);
               var response = await client.ExecuteAsync(request);

               var json = JsonConvert.DeserializeObject(response.Content!);
               JToken result = JToken.Parse(json!.ToString()!);
               var metadata = result["metadata"]![0];
               var fileid = metadata!["fileid"];
               var path = metadata!["path"];
               var toPath = string.Empty;

               if (foldername == "Portfolio")
               {
                    toPath = $"/Public Folder/Portfolio/";
               }
               else
               {
                    toPath = $"/Public Folder/Gallery/{foldername}/";
               }
               // Folder id is possibly null
               await CopyToFolder(token!, path!.ToString(), fileid!.ToString(), folderid!, toPath);
               return fileid!.ToString();


          }

          public async Task<Task> CopyToFolder(string token, string frompath, string fileid, string tofolderid, string topath)
          {
               Console.WriteLine("Trying to copy to folder...");

               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("copyfile");
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("path", frompath);
               request.AddParameter("topath", topath);
               var response = await client.ExecuteAsync(request);
               Console.WriteLine(response.Content);
               await DeleteFileFromRoot(token, frompath);
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
               var response = await client.ExecuteAsync(request);
               Console.WriteLine(response.Content);
               return Task.CompletedTask;
          }

          [HttpPost("/api/pCloud/DelPicsFromPCloud")]
          public async Task<IActionResult> DeleteFromPCloud(DelPhotosModel[]? model, [FromQuery(Name = "token")] string? token)
          {
               StringBuilder sb = new StringBuilder();
               foreach (var item in model!)
               {
                    await DeleteFromPCloud(item.FileID!, token!, $"/Public Folder/Gallery/{item.AlbumName}");
               }
               if (Message is not null)
               {
                    return Ok(Message);
               }
               return Ok();
          }

          public async Task<string> DeleteFromPCloud(string fileid, string token, string path)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("deletefile");
               request.AddHeader("Authorization", $"Bearer {token}");
               request.AddParameter("fileid", fileid);
               var response = await client.ExecuteAsync(request);
               Console.WriteLine(response.Content);
               Message = response.Content!;
               return response.Content!;
          }

          [HttpPost("/api/pCloud/RenameFolder")]
          public async Task<IActionResult> RenameFolder(string? folderid, string? token, [FromQuery(Name = "name")] string? name)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("renamefolder");
               request.AddParameter("access_token", token);
               request.AddParameter("folderid", folderid);
               request.AddParameter("toname", name);
               var response = await client.ExecuteAsync(request);
               return Ok(response.Content);
          }

          [HttpPost("/api/pCloud/DeleteFolder")]
          public async Task<IActionResult> DeleteFolder([FromQuery(Name = "folderid")] string? folderid, [FromQuery(Name = "token")] string? token)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("deletefolder");
               request.AddParameter("access_token", token);
               request.AddParameter("folderid", folderid);
               var response = await client.ExecuteAsync(request);
               return Ok(response.Content);
          }
     }
}



