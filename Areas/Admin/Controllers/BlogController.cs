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
#nullable enable
    [Area("Admin")]

    public class BlogController : Controller
    {

        private readonly ApplicationDbContext _db = default!;

        public BlogController(ApplicationDbContext db)
        {
            _db = db;

        }

        [Route("Admin/Blog")]
        public IActionResult Index()
        {
            return View(_db.Posts.ToList());
        }

        [Route("/Admin/Blog/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Blog/SavePost")]
        public async Task<IActionResult> SaveBlog(Posts model, IFormFile file)
        {
            var filePath = Path.Combine("Uploads", file.FileName);
            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError("", "File already exists.");
            }
            else
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                if (ModelState.IsValid)
                {
                    var b = new Posts();
                    b.PostContent = Request.Form["txtPost"];
                    b.PostExcerpt = model.PostExcerpt;
                    b.PostTitle = model.PostTitle;
                    b.CategoryID = model.CategoryID;
                    b.PostAuthor = model.PostAuthor;
                    b.PostLocation = model.PostLocation;
                    b.FeaturedImage = file.FileName;
                    b.PostDate = DateTime.Now;
                    b.PostIP = HttpContext.Connection.RemoteIpAddress!.ToString();
                    b.PostActive = "No";
                    _db.Posts.Add(b);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            

           
            return View("Create", model); // Works when nothing else will.  Makes sense because it's in the root Blog folder.
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
                b!.PostContent = model.PostContent;
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
            var b = _db.Posts.Where(b => b.PostID == id).FirstOrDefault()!;
            _db.Remove<Posts>(b);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(_db.Posts.Where(b => b.PostID == id).FirstOrDefault());
        }
    }
}