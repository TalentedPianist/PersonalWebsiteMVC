using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
    public class CommentsController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; } 
        public CommentsController(ApplicationDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }


        public IActionResult Index(int id)
        {
            var model = _db.Comments.Where(c => c.PostID == id).ToList();
            return View(model);
        }

        [Route("Comments/AddComment")]
        public IActionResult AddComment(BlogCommentViewModel model)
        {
            Comments comment = new Comments();
            comment.CommentAuthor = model.Comment.CommentAuthor;
            comment.CommentAuthorEmail = model.Comment.CommentAuthorEmail;
            comment.CommentAuthorUrl = model.Comment.CommentAuthorUrl;
            comment.CommentContent = model.Comment.CommentContent;
            comment.CommentDate = DateTime.Now;
            comment.CommentAuthorIP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
            comment.PostID = model.Comment.PostID;
            comment.CommentType = "Post";
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return RedirectToAction("SinglePost", new { controller = "Home", area = "Blog", id = model.Comment!.PostID });

        }
    }
}
