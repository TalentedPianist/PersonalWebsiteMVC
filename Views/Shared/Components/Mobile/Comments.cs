using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Views.Shared.Components.Mobile
{
   [ViewComponent]
    public class Comments : ViewComponent
    {
        private ApplicationDbContext _db { get; set; }
        public Comments(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke(int id)
        {
            var model = new MixModel();
            model.AllComments = _db.Comments.Where(c => c.PostID == id).ToList();
            model.Comments = new Models.Comments();
            return View("~/Views/Shared/Components/Mobile/Comments.cshtml", model);
        }
    }
}
