using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using Wangkanai.Extensions;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
    [Area("pCloud")]
    [Authorize(Roles="Admin")]
    public class HomeController : Controller
    {
          public ApplicationDbContext _db { get; set; }
          public IHttpClientFactory _httpClientFactory { get; }

          public HomeController(IHttpClientFactory httpClientFactory, ApplicationDbContext db)
          {
               _httpClientFactory = httpClientFactory;
               _db = db; 
          }

          [Route("/pCloud/")]
          public IActionResult Home()
          {
               return View("~/Areas/pCloud/Views/Albums/Index.cshtml");
          }

          [HttpPost]
          [Route("/pCloud/GetThumbs")]
          public async Task<IActionResult> GetThumbs(int getauth, string username, string password)
          {
               var httpClient = _httpClientFactory.CreateClient();
               var response = await httpClient.GetAsync($"https://eapi.pcloud.com?getauth={getauth}&username={username}&password={password}");
               var content = await response.Content.ReadAsStringAsync();
               return Ok(content);

          }

          [HttpPost]
          [Route("/pCloud/Albums/AddMultipleToDb")]
          public IActionResult AddMultipleAlbumsToDb([FromBody] List<Album> data)
          {
               _db.Albums.AddRange(data);
               _db.SaveChanges();
               return Ok(data);
          }

          [HttpGet]
          [Route("/pCloud/Albums/CheckAlbumInDb")]
          public bool CheckAlbumInDb(string name)
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               if (album is not null)
                    return true;
               else
                    return false;
          }

          [HttpGet]
          [Route("/pCloud/Photos/CheckPhotoInDb")]
          public bool CheckPicInDb(string name)
          {
               var pic = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
               if (pic is not null)
                    return true;
               else
                    return false;
          }


          [HttpPost]
          [Route("/pCloud/Albums/GetID")]
          public IActionResult AlbumID(string name)
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               return Ok(album!.AlbumID);
          }

          [HttpGet]
          [Route("pCloud/Photos/GetID")]
          public IActionResult PhotoID(string name)
          {
               var photo = _db.Photos.Where(a => a.Name == name).FirstOrDefault();
               return Ok(photo!.PhotoID);
          }
    }
}
