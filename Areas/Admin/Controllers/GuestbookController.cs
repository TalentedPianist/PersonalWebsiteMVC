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
     [Authorize(Policy = "Admin")]
     public class GuestbookController : Controller
     {
          private readonly ApplicationDbContext _db;

          public GuestbookController(ApplicationDbContext db)
          {
               _db = db;
          }

          public IActionResult Index()
          {
               return View(_db.Guestbook.ToList());
          }

          public IActionResult Create()
          {
               return View();
          }

          [HttpPost]
          public IActionResult Create(Guestbook model)
          {
               if (ModelState.IsValid)
               {
                    var g = new Guestbook();
                    g.GuestbookContent = model.GuestbookContent;
                    g.GuestbookAuthor = model.GuestbookAuthor;
                    g.GuestbookAuthorEmail = model.GuestbookAuthorEmail;
                    g.GuestbookAuthorUrl = model.GuestbookAuthorUrl;
                    g.DatePosted = DateTime.Now;
                    g.GuestbookAuthorIP = HttpContext!.Connection.RemoteIpAddress!.ToString();
                    g.GuestbookApproved = "No";
                    _db.Add(g);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Update(int id)
          {
               return View(_db.Guestbook.Where(g => g.Id == id).FirstOrDefault());
          }

          [HttpPost]
          public IActionResult Update(int id, Guestbook model)
          {
               if (ModelState.IsValid)
               {
                    var entry = _db.Guestbook.Where(g => g.Id == id).FirstOrDefault();
                    entry!.GuestbookContent = model.GuestbookContent;
                    entry.GuestbookAuthor = model.GuestbookAuthor;
                    entry.GuestbookAuthorEmail = model.GuestbookAuthorEmail;
                    entry.GuestbookAuthorUrl = model.GuestbookAuthorUrl;
                    entry.GuestbookApproved = model.GuestbookApproved;
                    _db.Update(entry);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
               }
               return View(model);
          }

          public IActionResult Delete(int id)
          {
               var g = _db.Guestbook.Where(g => g.Id == id).FirstOrDefault();
               _db.Remove(g!);
               _db.SaveChanges();
               return RedirectToAction("Index");
          }

          public IActionResult Details(int id)
          {
               return View(_db.Guestbook.Where(g => g.Id == id).FirstOrDefault());
          }

     }
}