using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using X.PagedList.Extensions;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class HomeController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public int PostID { get; set; } = 0;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Areas/Blog/Views/Index")]
        public IActionResult Index()
        {                   
            return View();
        }

        [Route("Blog/SinglePost/{id}")]
        public IActionResult SinglePost(int id)
        { // https://www.codeproject.com/Articles/1108855/10-Ways-to-Bind-Multiple-Models-on-a-View-in-MVC
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            model.Comments = _db.Comments.Where(c => c.PostID == id).ToList(); 
            
            PostID = id;
            //Comments();
            return View(model);
        }

      
        public IActionResult Comments()
        {
            ViewBag.strId = PostID;
            var comments = _db.Comments.Where(c => c.PostID == PostID).ToList();
            ViewBag.CommentCount = comments.Count();
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Comments = comments;
            return PartialView("~/Areas/Blog/Views/Home/Comments.cshtml");
        }
    }
}
