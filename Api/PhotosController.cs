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
using RestSharp;
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
               return await _context.Photos.AsNoTracking().ToListAsync();
          }

          [HttpPost("/api/Photos/AddPhoto")]
          public async Task<ActionResult> AddPhoto(List<Photos>? model, [FromQuery(Name = "AlbumName")] string? AlbumName)
          {
               var album = _context.Albums.Any(a => a.Name == AlbumName);
               return Ok(album);
          }



          [HttpGet("/api/Photos/CheckExists")]
          public IActionResult MultiplePhotoExists(string? name, int? albumID)
          {
               var photo = _context.Photos.Where(p => p.Name == name && p.AlbumID == albumID).Any();
               return Ok(photo);
          }

          [HttpDelete("/api/Photos/DeleteMultiple")]
          public IActionResult DeleteMultipePhotosFromDb([FromBody] Photos[] model)
          {
               _context.Photos.AsNoTracking();
               _context.RemoveRange(model);
               _context.SaveChanges();
               return Ok(model);
          }

          [HttpPost("/api/Photos/AddMultiple")]
          public IActionResult AddMultiplePhotosToDb([FromBody] Photos[] model)
          {
               try
               {
                    _context.Photos.AddRange(model!);
                    _context.SaveChanges();
                    return Ok(model);
               }
               catch (InvalidOperationException ex)
               {
                    return Ok(ex.Message);
               }
          }

          [HttpGet("/api/Photos/GetID")]
          public IActionResult GetPhotoID([FromQuery(Name = "name")] string name)
          {
               
               var photo = _context.Photos.Where(p => p.Name == name).AsNoTracking().FirstOrDefault();
               if (photo is not null)
               {
                    return Ok(photo!.PhotoID);
               }
               else
               {
                    return NotFound();
               }

          }

          [HttpPost("/api/photos/AddSinglePhoto")]
          public IActionResult AddSinglePhoto(Photos model)
          {
               _context.Photos.Add(model);
               _context.SaveChanges();
               return Ok(model);
          }

          [HttpGet("/api/photos/GetOnePic")]
          public IActionResult GetOnePic(string? name)
          {
               return Ok(name);
          }

          [HttpPost("/api/photos/DelSinglePhoto")]
          public IActionResult DelSinglePhoto(Photos model)
          {
               var photo = _context.Photos.Where(p => p.PhotoID == model.PhotoID).FirstOrDefault();
               _context.Photos.Remove(photo!);
               _context.SaveChanges();
               return Ok("Photo successfully deleted.");
          }

          [HttpGet("/api/photos/GetThumb")]
          public IActionResult GetThumb([FromQuery(Name = "fileid")] string? fileid, string? token, string? size)
          {
               try
               {
                    var client = new RestClient("https://eapi.pcloud.com");
                    var request = new RestRequest("/getthumblink");
                    request.AddParameter("fileid", fileid);
                    request.AddParameter("size", size);
                    request.AddParameter("access_token", token);
                    var result = client.Execute(request);
                    return Ok(result.Content);
               }
               catch (ArgumentNullException)
               {
                    return Ok();
               }
          }

          [HttpPut("/api/photos/MakeCoverPhoto")]
          public IActionResult MakeCoverPhoto(string? fileid, string? name)
          {
               try
               {
                    var album = _context.Albums.Where(a => a.Name == name).FirstOrDefault();
                    album!.CoverPhoto = fileid;
                    _context.Albums.Update(album);
                    _context.SaveChanges();
                    return Ok(album);
               }
               catch (NullReferenceException)
               {
                    return Ok();
               }
          }

          [HttpGet("/api/photos/GetAlbum")]
          public async Task<ActionResult<PaginatedList<Photos>>> GetAlbum(int id, int pageIndex = 1, int pageSize = 10)
          {
               var photos = _context.Photos.Where(p => p.AlbumID == id).AsQueryable().AsNoTracking();
               var count = await photos.CountAsync();
               var items = await photos.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
               var route = HttpContext.Request.Path.Value;
               route = route!.Replace("/api/", "");
               var result = new PaginatedList<Photos>(items, count, pageIndex, pageSize, route);
               return Ok(result);
          }

       
     }
}
