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
        public IActionResult Index()
        {
            return View();
        }

        [Route("Blog/SinglePost/{id}")]
        public IActionResult SinglePost(int id, [FromQuery(Name="pageNumber")]int? page)
        { // https://www.codeproject.com/Articles/1108855/10-Ways-to-Bind-Multiple-Models-on-a-View-in-MVC
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            //model.Comments = _db.Comments.Where(p => p.PostID == id).ToList();

            // Begin code for paginating comments
            
            var pageNumber = page ?? 1;
            var comments = _db.Comments.Where(c => c.PostID == id).ToList();
            var pagedComments = comments.ToPagedList(pageNumber, 1);
            model.PagedComments = (X.PagedList.PagedList<Comments>?)pagedComments;
            // End code for paginating comments
            PostID = id;
            Comments();
            return View(model);
        }

        public IActionResult Comments()
        {
            ViewBag.strId = PostID;
            return PartialView("~/Areas/Blog/Views/Home/Comments.cshtml");
        }
    }
}
