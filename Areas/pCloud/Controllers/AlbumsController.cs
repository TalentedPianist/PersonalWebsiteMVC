using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text.Json;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers;

[Area("pCloud")]
[Authorize(Roles="Admin")]
public class AlbumsController : Controller
{
    private IHttpClientFactory _httpClientFactory { get; set; }
     public ApplicationDbContext _db { get; set; }
     public IHttpContextAccessor _http { get; set; } 

    public AlbumsController(IHttpClientFactory httpClientFactory, ApplicationDbContext db, IHttpContextAccessor http)
    {
        _httpClientFactory = httpClientFactory;
          _db = db;
          _http = http;
    }

    public IActionResult Index()
    {
        
        return View();
    }


     [HttpGet]
     [Route("/pCloud/Albums/GetCoverPic")]
     public IActionResult GetCoverPic(string name)
     {
          try
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               return Ok(album!.CoverPhoto);
          }
          catch (NullReferenceException ex)
          {
               return NotFound(ex.Message);
          }
     }

     [HttpGet]
     [Route("/pCloud/Albums/GetID")]
     public IActionResult GetID(string name)
     {
          try
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               return Ok(album!.AlbumID);
          }
          catch (NullReferenceException ex)
          {
               return Ok(ex.Message);
          }
     }

     [HttpGet]
     [Route("/pCloud/Albums/CheckInDb")]
     public IActionResult IsInDb(string name)
     {
          var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault(); 
          if (album is not null)
               return Ok(true);
          else
               return Ok(false); 
     }

     [HttpPost]
     [Route("/pCloud/Albums/AddMultipleAlbumsToDb")]
     public IActionResult AddMultipleAlbumsToDb([FromBody]List<Album> data)
     {
          _db.Albums.AddRange(data);
          _db.SaveChanges();
          return Ok(data);
     }

     [HttpPost]
     [Route("/pCloud/Albums/DelMultipleFromDb")]
     public IActionResult DelMultipleFromDb([FromBody]List<Album> data)
     {
          try
          {
               _db.Albums.RemoveRange(data);
               _db.SaveChanges();
               return Ok(data);
          }
          catch (InvalidOperationException ex)
          {
               return Ok(data);
          }
     }

     [HttpPost]
     [Route("/pCloud/Albums/MakeCoverPic")]
     public IActionResult MakeCoverPic(string name, string url)
     {
          var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
          album!.CoverPhoto = url;
          _db.Albums.Update(album);
          _db.SaveChanges();
          return RedirectToAction("Index", new { Area = "pCloud", Controller = "Albums" });
     }
}