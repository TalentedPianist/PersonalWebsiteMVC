using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using PersonalWebsiteMVC.Data;
using System.Text.Json;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers;

[Area("pCloud")]
public class AlbumsController : Controller
{
    private IHttpClientFactory _httpClientFactory { get; set; }
     public ApplicationDbContext _db { get; set; }

    public AlbumsController(IHttpClientFactory httpClientFactory, ApplicationDbContext db)
    {
        _httpClientFactory = httpClientFactory;
          _db = db;
    }

    public IActionResult Index()
    {
        
        return View();
    }


     [HttpGet]
     [Route("/pCloud/Albums/GetCoverPic")]
     public IActionResult GetCoverPic(string name)
     {
          var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
          return Ok(album!.CoverPhoto);
     }

     [HttpGet]
     [Route("/pCloud/Albums/GetID")]
     public IActionResult GetID(string name)
     {
          var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
          return Ok(album!.AlbumID);
     }

}