using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using X.PagedList.Extensions;

namespace PersonalWebsiteMVC.Controllers
{
     public class PortfolioController : Controller
     {
          public ApplicationDbContext _db { get; set; }

          public PortfolioController(ApplicationDbContext db)
          {
               _db = db;
          }

          public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
          {
               var pageNumber = page ?? 1;
               var model = _db.Portfolio.OrderByDescending(p => p.DateCreated).ToPagedList(pageNumber, 10);
               return View("~/Views/Home/Portfolio.cshtml", model);
          }
     }
}
