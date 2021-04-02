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
    public class CommentsController : Controller
    {
          private readonly ApplicationDbContext _db;

          public CommentsController(ApplicationDbContext db)
          {
               _db = db; 
          }

        public IActionResult Index()
        {
            return View(_db.Comments.ToList());
        }

          public IActionResult Create()
          {
               return View();
          }

          [HttpPost]
          public IActionResult Create(Comments model)
          {
               if (ModelState.IsValid)
               {
                    var comment = new Comments();
                    comment.ParentID = 0;
                    comment.CommentContent = model.CommentContent;
                    comment.CommentAuthor = model.CommentAuthor;
                    comment.CommentAuthorEmail = model.CommentAuthorEmail;
                    comment.CommentAuthorUrl = model.CommentAuthorUrl;
                    comment.CommentDate = DateTime.Now;
                    comment.CommentAuthorIP = HttpContext.Connection.RemoteIpAddress.ToString();
                    _db.Comments.Add(comment);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Update(int id)
          {
               return View(_db.Comments.Where(c => c.CommentID == id).FirstOrDefault());
          }

          [HttpPost]
          public IActionResult Update(int id, Comments model)
          {
               if (ModelState.IsValid)
               {
                    var comment = _db.Comments.Where(c => c.CommentID == id).FirstOrDefault();
                    comment.ParentID = model.ParentID;
                    comment.CommentContent = comment.CommentContent;
                    comment.CommentAuthor = model.CommentAuthor;
                    comment.CommentAuthorEmail = model.CommentAuthorEmail;
                    comment.CommentAuthorUrl = model.CommentAuthorUrl;
                    _db.Update(comment);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Delete(int id)
          {
               var comment = _db.Comments.Where(c => c.CommentID == id).FirstOrDefault();
               _db.Remove(comment);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }

          public IActionResult Details(int id)
          {
               return View(_db.Comments.Where(c => c.CommentID == id).FirstOrDefault());
          }
    }
}