using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
    public class CommentsController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }
        private IConfiguration _configuration { get; set; }
        public ILogger<CommentsController> _Logger { get; set; }
        private readonly HttpClient _httpClient;

        public CommentsController(ApplicationDbContext db, IHttpContextAccessor http, IConfiguration config, ILogger<CommentsController> logger, HttpClient httpClient)
        {
            _db = db;
            _http = http;
            _configuration = config;
            _Logger = logger;
            _httpClient = httpClient;

        }

        [Route("Blog/Comments/AddComment/{id}")]
        public IActionResult Index([FromQuery(Name="pageNumber")]int? page)
        {
            var model = new BlogCommentViewModel();
            TempData["Message"] = ViewData["id"];     
            //model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            return View("~/Areas/Blog/Views/Shared/SinglePost.cshtml", model);
        }

     
        [HttpPost]
        [Route("Blog/Comments/AddComment/{id}")]
        public IActionResult AddComment(BlogCommentViewModel model)
        {
            TempData["Message"] = model.Comment.CommentAuthor;
            if (ModelState.IsValid)
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
                return RedirectToAction("SinglePost", new { area = "Blog", controller="Home", id = model.Comment.PostID });

            }
            return View("~/Areas/Blog/Views/Shared/SinglePost.cshtml", model);
        }
    }
}
