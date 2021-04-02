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
     public class CategoriesController : Controller
     {
          private readonly ApplicationDbContext _db; 

          public CategoriesController(ApplicationDbContext db)
          {
               _db = db; 
          }

          public IActionResult Index()
          {
               return View(_db.Categories);
          }

          public IActionResult Create()
          {
               return View();
          }

          [HttpPost]
          public IActionResult Create(Categories model)
          {
               if (ModelState.IsValid)
               {
                    var category = new Categories();
                    category.PostID = model.PostID; 
                    category.PostCount = model.PostCount; 
                    category.Category = model.Category; 
                    category.IP = model.IP;
                    category.CategoryDate = model.CategoryDate; 
                    _db.Categories.Add(category);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Update(int id)
          {
               return View(_db.Categories.Where(c => c.CategoryID == id).FirstOrDefault());
          }

          [HttpPost]
          public IActionResult Update(PersonalWebsiteMVC.Models.Categories model, int id)
          {
               if (ModelState.IsValid)
               {
                    var category = _db.Categories.Where(c => c.CategoryID == id).FirstOrDefault();
                    category.Category = model.Category;
                    _db.Categories.Update(category);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Details(int id)
          {
               return View(_db.Categories.Where(c => c.CategoryID == id).FirstOrDefault());
          }

          public IActionResult Delete(int id)
          {
               var category = _db.Categories.Where(c => c.CategoryID == id).FirstOrDefault();
               _db.Categories.Remove(category);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }

     }
}
