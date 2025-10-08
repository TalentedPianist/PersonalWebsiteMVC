using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using System.Text;
using SharpCompress;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     
     [Area("pCloud")]
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

          [Route("/pCloud/Photos/{id}/{name}")]
          public IActionResult Index(string id, string name)
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
          [Route("/pCloud/photos/AddMultipleToDb")]
          public IActionResult AddMultipleToDb([FromBody]List<PersonalWebsiteMVC.Models.Photos> data)
          {
               _db.Photos.AddRange(data);
               _db.SaveChanges();
               
               return Ok(data);
          }
     }
}
