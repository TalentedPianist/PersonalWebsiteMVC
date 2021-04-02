using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Authorize(Policy = "Admin")]
     public class BlogController : Controller
     {
          private readonly ApplicationDbContext _db;

          public BlogController(ApplicationDbContext db)
          {
               _db = db;
          }

          public IActionResult Index()
          {
               return View(_db.Posts.ToList());
          }

          public IActionResult Create()
          {
               return View();
          }

          [HttpPost]
          public IActionResult Create(Posts model)
          {
               if (ModelState.IsValid)
               {
                    var b = new Posts();
                    b.PostContent = model.PostContent;
                    b.PostExcerpt = model.PostExcerpt;
                    b.PostTitle = model.PostTitle;
                    b.CategoryID = model.CategoryID; 
                    b.PostAuthor = model.PostAuthor;
                    b.PostLocation= model.PostLocation;
                    b.PostDate = DateTime.Now;
                    b.PostIP = HttpContext.Connection.RemoteIpAddress.ToString();
                    b.PostActive = "No";
                    _db.Posts.Add(b);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Update(int id)
          {
          
               return View(_db.Posts.Where(b => b.PostID == id).FirstOrDefault());
          }

          [HttpPost]
          public IActionResult Update(int id, Posts model)
          {
               if (ModelState.IsValid)
               {
                    var b = _db.Posts.Where(b => b.PostID == id).FirstOrDefault();
                    b.PostContent = model.PostContent;
                    b.PostExcerpt = model.PostExcerpt;
                    b.PostTitle = model.PostTitle;
                    b.PostAuthor = model.PostAuthor;
                    b.PostLocation = model.PostLocation;
                    b.CategoryID = model.CategoryID;
                    _db.Update(b);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Delete(int id)
          {
               var b = _db.Posts.Where(b => b.PostID == id).FirstOrDefault();
               _db.Remove(b);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }

          public IActionResult Details(int id)
          {
               return View(_db.Posts.Where(b => b.PostID == id).FirstOrDefault());
          }
     }
}