using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using X.PagedList.Extensions;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using X.PagedList;

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
            // Spending ages editing the wrong function and wondering why nothing's happening is something I will need to watch.
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.PagedPosts = _db.Posts.ToPagedList(1, 1);

            return View("~/Areas/Blog/Views/Index.cshtml", model);
        }

        [Route("Blog/SinglePost/{id}")]
        public IActionResult SinglePost(int? id, [FromQuery(Name="pageNumber")]int? page)
        { // https://www.codeproject.com/Articles/1108855/10-Ways-to-Bind-Multiple-Models-on-a-View-in-MVC
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            model.Comments = _db.Comments.Where(c => c.PostID == id).ToList();
            // Just spent hours editing the wrong function.  No wonder it's always null.  What an eejit.
      
            //var pageNumber = page ?? 1;
            ViewBag.OnePageOfPosts = _db.Posts.ToPagedList(1, 1);
            // The problem was in the model.  It needs to be IPagedList instead of PagedList.  This is the sort of thing you could waste so much time on.
            model.PagedPosts = _db.Posts.ToPagedList<Posts>(1, 1);
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
