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
          public IActionResult GetAlbums()
          {
               return Ok(_db.Albums);
          }

          [HttpGet("/api/albums/Album/{id}")]
          public IActionResult Update(int id)
          {
               var album = _db.Albums.Where(a => a.AlbumID == id).FirstOrDefault();
               return Ok(album);
          }


          [HttpPut("/api/albums/UpdateAlbum/{id}")]
          public IActionResult UpdateAlbum([FromBody]Album model)
          {
               var album = _db.Albums.Where(a => a.AlbumID == model.AlbumID).FirstOrDefault();
               _db.Albums.Update(album!);
               _db.SaveChanges();
               return Ok(model);
          }

          [HttpGet("/api/albums/GetID")]
          public IActionResult GetID(string name)
          {
               var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
               return Ok(album!.AlbumID);
          }

          [HttpGet("/api/albums/PhotoExists")]
          public IActionResult PhotoExists(string name, int albumId)
          {
               return Ok(albumId);
          }
          
    }
}
