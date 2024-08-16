using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Views.Shared.Components.Mobile
{
    public class SinglePost : ViewComponent
    {
        
        private readonly IHttpContextAccessor _http;
        private readonly ApplicationDbContext _db;

        public SinglePost(IHttpContextAccessor httpContext, ApplicationDbContext db)
        {
            _http = httpContext;
            _db = db;
        }

      
        public IViewComponentResult Invoke(int? id)
        {
            // The parameter needs to be passed in the Component.InvokeAsync bit of the html, then it can be accessed here as above.
            var model = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
          
            return View("~/Views/Shared/Components/Mobile/SinglePost.cshtml", model);
        }
    }
}
