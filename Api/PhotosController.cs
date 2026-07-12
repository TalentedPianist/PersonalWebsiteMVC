using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class PhotosController : ControllerBase
     {
          private readonly ApplicationDbContext _context;

          public PhotosController(ApplicationDbContext context)
          {
               _context = context;
          }

          // GET: api/Photos
          [HttpGet]
          public async Task<ActionResult<IEnumerable<Photos>>> GetPhotos()
          {
               return await _context.Photos.ToListAsync();
          }

          [HttpPost("/api/Photos/AddPhoto")]
          public async Task<ActionResult> AddPhoto(List<Photos>? model, [FromQuery(Name="AlbumName")]string? AlbumName)
          {
               var album = _context.Albums.Any(a => a.Name == AlbumName);
               return Ok(album);
          }

          [HttpGet("/api/GetIP")]
          public IActionResult GetIP()
          {
               return Ok(HttpContext.Connection.RemoteIpAddress!.ToString());
          }

          [HttpGet("/api/Photos/PhotoExists")]
          public IActionResult PhotoExists(string photoName, int albumID)
          {
               var photo = _context.Photos.Where(p => p.Name == photoName && p.AlbumID == albumID).Any();
               return Ok(photo);
          }
     }
}
