using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
    public class CommentsController : Controller
    {
        public ApplicationDbContext _db { get; set; }

        public CommentsController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index(int id)
        {
            
            var model = _db.Comments.Where(c => c.PostID == id).ToList();   
            return View(model);
        }

    }
}
