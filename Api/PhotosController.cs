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
using SharpCompress;

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

          [HttpGet("/api/Photos/CheckExists")]
          public IActionResult MultiplePhotoExists(string? name, int? albumID)
          {
               var photo = _context.Photos.Where(p => p.Name == name).Any();
               return Ok(photo);
          }

          [HttpPost("/api/Photos/DeleteMultiple")]
          public IActionResult DeleteMultipePhotosFromDb(Photos[]? model)
          {
              
                    _context.Photos.RemoveRange(model);
                    _context.SaveChanges();
               
               return Ok(model);
          }

          [HttpPost("/api/Photos/AddMultiple")]
          public IActionResult AddMultiplePhotosToDb(Photos[]? model)
          {
               _context.Photos.AddRange(model!);
               _context.SaveChanges();
               return Ok(model);
          }

          [HttpGet("/api/Photos/GetID")]
          public IActionResult GetPhotoID([FromQuery(Name="name")]string name)
          {
               var photo = _context.Photos.Where(p => p.Name == name).FirstOrDefault();
               return Ok(photo!.PhotoID);
          }
     }
}
