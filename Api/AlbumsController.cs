using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Areas.pCloud.Helpers;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
          public IHttpClientFactory _httpClientFactory { get; set; }
          public IHttpContextAccessor _http { get; set; }
          public ApplicationDbContext _db { get; set; }
          public IWebHostEnvironment _env { get; set; }
          public IConfiguration _config { get; set; }

          public AlbumsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor http, ApplicationDbContext db, IWebHostEnvironment env, IConfiguration config)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
               _db = db;
               _env = env;
               _config = config;
          }


          [HttpGet("/api/albums/GetAlbum")]
          public async Task<ActionResult<PaginatedList<Album>>> GetAlbums(int pageIndex = 1, int pageSize = 10)
          {
               var albums = _db.Albums.AsQueryable().AsNoTracking();
               var count = await albums.CountAsync();
               var items = await albums.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
               return Ok(_db.Albums);
          }

          [HttpGet("/api/albums/Album/{id}")]
          public IActionResult Update(int id)
          {
               var album = _db.Albums.Where(a => a.AlbumID == id).FirstOrDefault();
               return Ok(album);
          }


          [HttpPut("/api/albums/UpdateAlbum/{id}")]
          public IActionResult UpdateAlbum(Album model)
          {
               // .AsNoTracking() prevents entity type instance errors
               var album = _db.Albums.Where(a => a.AlbumID == model.AlbumID).AsNoTracking().FirstOrDefault();
               if (album is not null)
               {
                    _db.Albums.Update(model);
                    _db.SaveChanges();
               }
               return Ok(album);
          }

          [HttpGet("/api/albums/GetID")]
          public IActionResult GetID([FromQuery(Name="name")]string? name)
          {
              
                    var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
                    return Ok(album);
               
          }

          [HttpGet("/api/albums/AlbumExists")]
          public IActionResult AlbumExists(string? name)
          {
               var album = _db.Albums.Where(a => a.Name == name).Any();
               return Ok(album);
          }
          
          

          [HttpPost("/api/albums/Create")]
          public IActionResult CreateAlbum(Album model)
          {
               _db.Albums.Add(model);
               _db.SaveChanges();
               return Ok(model);
          }

          [HttpDelete("/api/albums/Delete")]
          public IActionResult DeleteAlbum(string pCloudID)
          {
               var album = _db.Albums.Where(a => a.PCloudFolderID == pCloudID).FirstOrDefault();
               _db.Albums.Remove(album!);
               _db.SaveChanges();
               return Ok(pCloudID);
          }
    }
}
