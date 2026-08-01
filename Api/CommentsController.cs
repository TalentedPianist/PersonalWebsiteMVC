using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

          [Route("/api/Comment/Create")]
          public IActionResult CreatePost(Comments? model)
          {
               _db.Comments.Add(model!);
               _db.SaveChanges();
               return Ok(model);
          }

          [HttpGet("/api/Comment/{id}")]
          public IActionResult GetComment(int id)
          {
               var comment = _db.Comments.Where(c => c.CommentID == id).FirstOrDefault();
               return Ok(comment);
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
          public IActionResult BlogComments(int PostID)
          {
               var comments = _db.Comments.Where(c => c.PostID == PostID).OrderByDescending(c => c.CommentDate);
               return Ok(comments);
          }
     }
}
