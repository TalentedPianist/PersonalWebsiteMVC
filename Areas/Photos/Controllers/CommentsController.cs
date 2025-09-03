using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;

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
    }
}
