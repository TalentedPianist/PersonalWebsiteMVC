using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Views.Shared.Components;

[ViewComponent]
public class Blog : ViewComponent
{
    private ApplicationDbContext _db { get; set; }

    public Blog(ApplicationDbContext db)
    {
        _db = db;
    }

    public IViewComponentResult Invoke()
    {
        return View();
    }

   
}