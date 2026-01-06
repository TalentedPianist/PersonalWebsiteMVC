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

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     
     [Area("pCloud")]
     [Authorize(Roles="Admin")]
     public class PhotosController : Controller
     {
          public IHttpClientFactory _httpClientFactory { get; set; }
          public IHttpContextAccessor _http { get; set; }
          public ApplicationDbContext _db { get; set; }
          public string AccessToken { get; set; } = "655SZc96w0kZHSiMCoeCgGHDGzm9PXbVdHfvI1X0";
          public string HostName { get; set; } = "eapi.pcloud.com";

          public PhotosController(IHttpClientFactory httpClientFactory,  IHttpContextAccessor http, ApplicationDbContext db)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
               _db = db; 
          }

          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos")]
          public IActionResult Index([FromQuery(Name="id")] string id, [FromQuery(Name="name")]string name)
          {
               ViewBag.FolderId = id;
               ViewBag.Name = name;
               return View();
          }

          [HttpGet]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/IsInDb")]
          public IActionResult IsInDb(string name)
          {
               var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
               if (photo is not null)
                    return Ok(true);
               else
                    return Ok(false);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/AddMultipleToDb")]
          public IActionResult AddMultipleToDb([FromBody]List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.AddRange(data);
               _db.SaveChanges();
               
               return Ok(data);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/RemoveMultipleFromDb")]
          public IActionResult DelMultipleFromDb([FromBody]List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.RemoveRange(data);
               _db.SaveChanges();
               return Ok(data);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/GetID")]
          public IActionResult GetID([FromForm(Name="name")]string name)
          {
               var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
               return Ok(photo!.PhotoID);
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/Popup")]
          public IActionResult Popup([FromForm(Name="url")]string image, [FromForm(Name="name")]string name, [FromForm(Name="albumName")]string albumName, [FromForm(Name="albumId")]string albumId)
          {
               ViewBag.Image = image;
               ViewBag.Name = name;
               ViewBag.AlbumName = albumName;
               ViewBag.AlbumId = albumId;
               return PartialView("~/Areas/pCloud/Views/Photos/Popup.cshtml");
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/MakeCoverPic")]
          public IActionResult MakeCoverPic([FromForm(Name="CoverPic")] string CoverPic, [FromForm(Name="AlbumName")]string AlbumName)
          {
               var album = _db.Albums.Where(a => a.Name == AlbumName).FirstOrDefault();
               album!.CoverPhoto = CoverPic;
               _db.Albums.Update(album);
               _db.SaveChanges();
               return RedirectToAction("Index", new { area = "pCloud", controller = "Albums" });
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/Auth")]
          public IActionResult PCloudAuth()
          {
               string clientId = "GJR8uDME26u";

               string url = $"https://my.pcloud.com/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri=http://localhost:5051/pCloud/";
               return Redirect(url);
          }

          public IActionResult GetAccessToken([FromQuery(Name="code")]string code)
          {

               string clientId = "GJR8uDME26u";
               string clientSecret = "U83OQca6ABpaiDtaBsStUbgKRiAk";
               string url = "https://api.pcloud.com/oauth2_token";
               if (code is not null)
               {
                    var client = new RestClient(url);
                    var request = new RestRequest();
                    request.AddParameter("client_id", clientId);
                    request.AddParameter("client_secret", clientSecret);
                    request.AddParameter("code", code);
                    var response = client.Execute(request);
                    return Ok(response);
               }
               return Ok();
          }

          [HttpPost]
          [Microsoft.AspNetCore.Mvc.Route("/pCloud/Photos/GetUser")]
          public async Task<IActionResult> UserInfo()
          {
               var client = new RestClient("https://api.pcloud.com/userinfo");
               var request = new RestRequest();
               request.AddHeader("Bearer", AccessToken);
               var response = client.Execute(request);
               return Ok(response);
          }

     }
}
