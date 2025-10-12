using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using ServiceStack.Text;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Area("Admin")]
     public class PortfolioController : Controller
     {
          public ApplicationDbContext _db { get; set; }
          public IHttpContextAccessor _http { get; set; }

          private UserManager<ApplicationUser> userManager;

          public PortfolioController(ApplicationDbContext db, IHttpContextAccessor http, UserManager<ApplicationUser> userMgr)
          {
               _db = db;
               _http = http;
               userManager = userMgr;
          }

          public IActionResult Index()
          {
               return View(_db.Portfolio.ToList());
          }

          public IActionResult Create()
          {
               return View();
          }

          public IActionResult Update(int id)
          {
               return View(_db.Portfolio.Where(p => p.PortfolioID == id).FirstOrDefault());
          }

          public IActionResult Details(int id)
          {
               return View(_db.Portfolio.Where(p => p.PortfolioID == id).FirstOrDefault());
          }

          public IActionResult Delete(int id)
          {
               return View(_db.Portfolio.Where(p => p.PortfolioID == id).FirstOrDefault());
          }

          [HttpPost]
          [Route("/Admin/Portfolio/AddPortfolio")]
          public async Task<IActionResult> AddPortfolio(Portfolio model)
          {
               var user = await userManager.GetUserAsync(_http.HttpContext!.User);
               var portfolio = new Portfolio();
               portfolio.Name = model.Name;
               portfolio.Url = model.Url;
               portfolio.Description = model.Description;
               portfolio.ImageUrl = model.ImageUrl;
               portfolio.DateCreated = DateTime.Now;
               portfolio.IP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
               // If user is null it means user needs to log in again!
               portfolio.User = $"{user!.FirstName} {user.LastName}";
               _db.Portfolio.Add(portfolio);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }

          [HttpPost]
          [Route("/Admin/Portfolio/UpdatePortfolio")]
          public IActionResult UpdatePortfolio(Portfolio model, [FromForm(Name="portfolioID")]int PortfolioID)
          {
               var portfolio = _db.Portfolio.Where(p => p.PortfolioID == PortfolioID).FirstOrDefault();
               portfolio!.Name = model.Name;
               portfolio.Url = model.Url;
               portfolio.Description = model.Description;
               portfolio.ImageUrl = model.ImageUrl;
               _db.Portfolio.Update(portfolio);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }

          [HttpPost]
          [Route("/Admin/Portfolio/DeletePortfolio")]
          public IActionResult DeletePortfolio([FromForm(Name="portfolioID")]int PortfolioID)
          {
               var portfolio = _db.Portfolio.Where(p => p.PortfolioID == PortfolioID).FirstOrDefault();
               _db.Portfolio.Remove(portfolio!);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }
     }
}
