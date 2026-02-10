using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
     public class CommentsController : Controller
     {
          public ApplicationDbContext _db;
          public IHttpContextAccessor _http;
          public CommentsController(ApplicationDbContext db, IHttpContextAccessor http)
          {
               _db = db;
               _http = http;
          }

          public IActionResult Index()
          {
               return View();
          }

          [HttpPost]
          [Route("GetComments")]
          public IActionResult GetComments(int id)
          {
               var comments = _db.Comments.
                   Where(c => c.PostID == id)
                   .OrderByDescending(c => c.CommentDate)
                   .ToList();
               return Ok(comments);
          }

          [HttpPost]
          [Route("/Album/Comments/AddComment")]
          public IActionResult AddComment(Comments model)
          {
               Comments comment = new Comments();
               comment.PhotoID = model.PhotoID;
               comment.CommentAuthor = model.CommentAuthor;
               comment.CommentAuthorEmail = model.CommentAuthorEmail;
               comment.CommentAuthorUrl = model.CommentAuthorUrl;
               comment.CommentContent = model.CommentContent;
               comment.CommentAuthorIP = HttpContext.Connection.RemoteIpAddress!.ToString();
               comment.CommentDate = DateTime.Now;
               _db.Comments.Add(comment);
               _db.SaveChanges();
               return Ok(comment);
          }

          [Route("/Album/GetComments")]
          [HttpPost]
          public IActionResult GetComments(int id, int page = 1)
          {
               //int pageSize = 1;
               var comments = _db.Comments
                   .Where(c => Convert.ToInt32(id) == Convert.ToInt32(c.PhotoID))
                   //.OrderByDescending(c => c.CommentDate)
                   .ToList();
                                   


               return PartialView("~/Areas/Photos/Views/Album/Comments.cshtml", comments);
          }

         
     }
}
