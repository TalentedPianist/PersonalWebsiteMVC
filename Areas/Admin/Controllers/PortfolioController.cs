using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Area("Admin")]
     public class PortfolioController : Controller
     {
          public ApplicationDbContext _db { get; set; }

          public PortfolioController(ApplicationDbContext db)
          {
               _db = db;
          }

          public IActionResult Index()
          {
               return View();
          }
     }
}
