using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using Microsoft.AspNetCore.Http;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _http;

        public CommentsController(ApplicationDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        [Route("Admin/Comments")]
        public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
        {
            var comments = _db.Comments;
            var pageNumber = page ?? 1;
            var model = comments.ToPagedList(pageNumber, 1);
            return View(model);
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
                    comment.PostID = 0;
                    comment.CommentContent = model.CommentContent;
                    comment.CommentAuthor = model.CommentAuthor;
                    comment.CommentAuthorEmail = model.CommentAuthorEmail;
                    comment.CommentAuthorUrl = model.CommentAuthorUrl;
                    comment.CommentDate = DateTime.Now;
                    comment.CommentAuthorIP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
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
                    comment!.PostID = model.PostID;
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
               _db.Remove(comment!);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }

          public IActionResult Details(int id)
          {
               return View(_db.Comments.Where(c => c.CommentID == id).FirstOrDefault());
          }
    }
}