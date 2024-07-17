using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class BlogController : Controller
    {

        private readonly ApplicationDbContext _db = default!;

        public BlogController(ApplicationDbContext db)
        {
            _db = db;

        }

        [Microsoft.AspNetCore.Mvc.Route("Admin/Blog")]
        public IActionResult Index([FromQuery(Name = "pageNumber")]int? page)
        {
            var posts = _db.Posts;
            var pageNumber = page ?? 1;
            var model = posts.ToPagedList(pageNumber, 5);
            return View(model);
        }

        [Microsoft.AspNetCore.Mvc.Route("Admin/Blog/Create")]
        [Microsoft.AspNetCore.Mvc.Route("Blog/SavePost")]
        public IActionResult Create()
        {
            return View();
        }

       

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Blog/SavePost")]
        public async Task<IActionResult> SavePost(Posts model, [FromForm(Name="File1")]IFormFile file)
        {
            // This wasn't working because IFormFile was null.  Checking for null with a conditional fixed the problem.
            if (file is not null)
            {
                var filePath = file.FileName;

                if (System.IO.File.Exists("wwwroot/Uploads/" + file.FileName))
                {
                    ModelState.AddModelError("", "File already exists.");
                }
                else
                {
                    using (var stream = System.IO.File.Create("wwwroot/Uploads/" + filePath))
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
            }
            else
            {
                ModelState.Clear(); // Clear the model state to avoid file field is required error
                if (ModelState.IsValid)
                {
                    var b = new Posts();
                    b.PostContent = Request.Form["txtPost"];
                    b.PostExcerpt = model.PostExcerpt;
                    b.PostTitle = model.PostTitle;
                    b.CategoryID = model.CategoryID;
                    b.PostAuthor = model.PostAuthor;
                    b.PostLocation = model.PostLocation;
                    b.PostDate = DateTime.Now;
                    b.PostIP = HttpContext.Connection.RemoteIpAddress!.ToString();
                    b.PostActive = "No";
                    _db.Posts.Add(b);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View("Create", model); // View only needs the file name to work without any other paths or extensions.
        }


        public IActionResult Update(int id)
        {

            return View(_db.Posts.Where(b => b.PostID == id).FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Posts model, [FromForm(Name="File1")]IFormFile file, [FromForm(Name="txtPost")]string post)
        {
            if (file == null)
            {
                TempData["Message"] = post;
            }
            else
            {
                if (System.IO.File.Exists("wwwroot/Uploads/" + file.FileName))
                {
                    ModelState.Clear(); // Reset model state because it is valid
                }
                else
                {
                    using (var stream = System.IO.File.Create("wwwroot/Uploads/" + file.FileName))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ModelState.Clear();
                }
            }

            if (ModelState.IsValid)
            {
                var b = _db.Posts.Where(b => b.PostID == id).FirstOrDefault();
                b!.PostContent = post;
                b.PostExcerpt = model.PostExcerpt;
                b.PostTitle = model.PostTitle;
                b.FeaturedImage = file!.FileName;
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