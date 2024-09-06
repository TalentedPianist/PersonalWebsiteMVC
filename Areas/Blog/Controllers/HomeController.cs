using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using X.PagedList.Extensions;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class HomeController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public int PostID { get; set; } = 0;
        private IConfiguration _configuration;
        public HomeController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _configuration = config;
        }

        [Route("Areas/Blog/Views/Index")]
        public IActionResult Index()
        {
            TempData["Message"] = DateTime.Now;
            return View("~/Areas/Blog/Views/Index.cshtml");
        }

        [Route("Blog/SinglePost/{id}")]
        public IActionResult SinglePost(int id, [FromQuery(Name="pageNumber")]int? page)
        { // https://www.codeproject.com/Articles/1108855/10-Ways-to-Bind-Multiple-Models-on-a-View-in-MVC
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            model.Comments = _db.Comments.Where(c => c.PostID == id).ToList();

            var posts = _db.Posts;
            var pageNumber = page ?? 1;
            model.PagedPosts = (X.PagedList.PagedList<Posts>?)posts.ToPagedList(pageNumber, 1);
          
            ViewBag.PostID = id;
            return View(model);
        }

      
        public IActionResult Comments()
        {
            ViewBag.strId = PostID;
            var comments = _db.Comments.Where(c => c.PostID == PostID).ToList();
            ViewBag.CommentCount = comments.Count();
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Comments = comments;
            ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;

            return PartialView("~/Areas/Blog/Views/Home/Comments.cshtml");
        }
    }
}
