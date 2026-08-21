using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class CommentsController : ControllerBase
     {
          public ApplicationDbContext _db { get; set; }

          public CommentsController(ApplicationDbContext db)
          {
               _db = db;
          }


          [HttpGet("/api/Comments")]
          public IActionResult GetComments()
          {
               return Ok(_db.Comments);
          }

          [HttpPost("/api/Comment/Create")]
          public IActionResult CreatePost(Comments? model)
          {
               if (ModelState.IsValid)
               {
                    model!.CommentDate = DateTime.Now;
                    model.CommentAuthorIP = HttpContext.Connection.RemoteIpAddress!.ToString();
                    _db.Comments.Add(model!);
                    _db.SaveChanges();
                    return Ok(model);
               }
               else
               {
                    return Ok();
               }

          }

          [HttpGet("/api/PostComments/{id}")]
          public async Task<ActionResult<PaginatedList<Comments>>> GetComment(int id, int pageIndex = 1, int pageSize = 1)
          {
               var comments = _db.Comments.AsQueryable().AsNoTracking();
               var count = await comments.CountAsync();
               var items = await comments.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

               var route = HttpContext.Request.Path.Value;
               route = route!.Replace("/api/", "");
               var result = new PaginatedList<Comments>(items, count, pageIndex, pageSize, route!);

               return Ok(result);
          }

          [HttpPut("/api/Comment/Update/")]
          public IActionResult UpdateComment(Comments model)
          {
               _db.Comments.Update(model);
               _db.SaveChanges();
               return Ok(model);
          }

          [HttpDelete("/api/Comment/{id}")]
          public IActionResult DeleteComment(int id)
          {
               var comment = _db.Comments.Where(c => c.CommentID == id).FirstOrDefault();
               _db.Comments.Remove(comment!);
               _db.SaveChanges();
               return Ok(id);
          }

          [HttpGet("/api/Comments/GetComments")]
          public IActionResult BlogComments(int? PostID, int? PhotoID)
          {

               var comments = _db.Comments.Where(c => c.PostID == PostID).OrderByDescending(c => c.CommentDate);
               return Ok(comments);

          }

          [HttpGet("/api/Comments/PhotoComments")]
          public async Task<ActionResult<PaginatedList<Comments>>> PhotoComments(int PhotoID)
          {
               var id = Convert.ToInt32(PhotoID);
               
               var comments = _db.Comments.Where(c => c.PhotoID!.Equals(PhotoID));
               return Ok(comments);
          }
     }
}
