using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using System.Text;
using SharpCompress;
using PersonalWebsiteMVC.Data;
using Microsoft.AspNetCore.Authorization;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     
     [Area("pCloud")]
     [Authorize(Roles="Admin")]
     public class PhotosController : Controller
     {
          public IHttpClientFactory _httpClientFactory { get; set; }
          public IHttpContextAccessor _http { get; set; }
          public ApplicationDbContext _db { get; set; }


          public PhotosController(IHttpClientFactory httpClientFactory,  IHttpContextAccessor http, ApplicationDbContext db)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
               _db = db; 
          }

          [Route("/pCloud/Photos")]
          public IActionResult Index([FromQuery(Name="id")] string id, [FromQuery(Name="name")]string name)
          {
               ViewBag.FolderId = id;
               ViewBag.Name = name;
               return View();
          }

          [HttpGet]
          [Route("/pCloud/Photos/IsInDb")]
          public IActionResult IsInDb(string name)
          {
               var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
               if (photo is not null)
                    return Ok(true);
               else
                    return Ok(false);
          }

          [HttpPost]
          [Route("/pCloud/Photos/AddMultipleToDb")]
          public IActionResult AddMultipleToDb([FromBody]List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.AddRange(data);
               _db.SaveChanges();
               
               return Ok(data);
          }

          [HttpPost]
          [Route("/pCloud/Photos/RemoveMultipleFromDb")]
          public IActionResult DelMultipleFromDb([FromBody]List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.RemoveRange(data);
               _db.SaveChanges();
               return Ok(data);
          }

          [HttpPost]
          [Route("/pCloud/Photos/GetID")]
          public IActionResult GetID([FromForm(Name="name")]string name)
          {
               var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
               return Ok(photo!.PhotoID);
          }

          [HttpPost]
          [Route("/pCloud/Photos/Popup")]
          public IActionResult Popup([FromForm(Name="url")]string image, [FromForm(Name="name")]string name, [FromForm(Name="albumName")]string albumName, [FromForm(Name="albumId")]string albumId)
          {
               ViewBag.Image = image;
               ViewBag.Name = name;
               ViewBag.AlbumName = albumName;
               ViewBag.AlbumId = albumId;
               return PartialView("~/Areas/pCloud/Views/Photos/Popup.cshtml");
          }

          [HttpPost]
          [Route("/pCloud/Photos/MakeCoverPic")]
          public IActionResult MakeCoverPic([FromForm(Name="CoverPic")] string CoverPic, [FromForm(Name="AlbumName")]string AlbumName)
          {
               var album = _db.Albums.Where(a => a.Name == AlbumName).FirstOrDefault();
               album!.CoverPhoto = CoverPic;
               _db.Albums.Update(album);
               _db.SaveChanges();
               return RedirectToAction("Index", new { area = "pCloud", controller = "Albums" });
          }
     }
}
