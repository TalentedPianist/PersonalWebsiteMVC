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
using X.PagedList.Extensions;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Authorize(Roles = "Admin")]
     [Area("Admin")]
     public class BlogController : Controller
     {

          private readonly ApplicationDbContext _db = default!;
          private IWebHostEnvironment _environment;

          public BlogController(ApplicationDbContext db, IWebHostEnvironment environment)
          {
               _db = db;
               _environment = environment;
          }

          [Microsoft.AspNetCore.Mvc.Route("Admin/Blog")]
          public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
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

          // https://www.c-sharpcorner.com/article/complete-file-upload-and-download-in-asp-net-core-mvc/
          [HttpPost]
          public async Task<IActionResult> CreatePost(Posts model)
          {
               if (model.FileUpload is not null)
               {
                    // Define the upload folder
                    string uploadPath = System.IO.Path.Combine(_environment.WebRootPath, "Uploads");
                    // Create the directory if it doesn't exist
                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                         System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    // Generate the file path
                    string filePath = System.IO.Path.Combine(uploadPath, model.FileUpload.FileName);
                    // Save the file to the specified location
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                         await model.FileUpload.CopyToAsync(stream);
                    }
                    model.FeaturedImage = model.FileUpload.Name;
               }
               
               _db.Posts.Add(model);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }


          public IActionResult Update(int id)
          {

               return View(_db.Posts.Where(b => b.PostID == id).FirstOrDefault());
          }

          [HttpPost]
          public async Task<IActionResult> UpdatePost(Posts model)
          {
               if (model.FileUpload is not null)
               {
                    string uploadPath = System.IO.Path.Combine(_environment.WebRootPath, "Uploads");
                    string filePath = System.IO.Path.Combine(uploadPath, model.FileUpload.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                         await model.FileUpload.CopyToAsync(stream);
                    }
                    model.FeaturedImage = model.FileUpload.FileName;
               }

               _db.Posts.Update(model);
               _db.SaveChanges();
               return View("Update", model);
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

          [HttpPost]
          public IActionResult SaveDraft([FromBody] Posts model)
          {
             
               _db.Posts.Add(model);
               _db.SaveChanges();
               return Ok(model);
          }

          [HttpPost]
          public IActionResult DeleteMultiple([FromBody] List<Posts> posts)
          {
               _db.Posts.RemoveRange(posts);
               _db.SaveChanges();
               return Ok();
          }
     }
}