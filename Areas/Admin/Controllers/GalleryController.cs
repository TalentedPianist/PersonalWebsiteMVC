using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Helpers;
using Microsoft.Graph;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using PersonalWebsiteMVC.Data;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Html;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Area("Admin")]
     public class GalleryController : Controller
     {
          private readonly IGraphSDKHelper _Graph;
          private readonly ApplicationDbContext _db;
          private readonly IWebHostEnvironment _env;
          private readonly IHttpContextAccessor _http;
          public GalleryController(IGraphSDKHelper graph, ApplicationDbContext db, IWebHostEnvironment env, IHttpContextAccessor http)
          {
               _Graph = graph;
               _db = db;
               _env = env;
               _http = http;
          }

          public IActionResult Index()
          {
               return View();
          }

     
          public IActionResult Create()
          {
               return View();
          }

          public IActionResult Album()
          {
               return View();
          }
          

          public IActionResult Update()
          {
               string id = _http.HttpContext.Request.Query["q"];
               string title = _http.HttpContext.Request.Query["title"];
               var query = _db.Gallery.Where(g => g.GalleryName == title && g.GalleryRemoteID == id).FirstOrDefault();
               var model = new PersonalWebsiteMVC.Models.Gallery();
               model.GalleryName = query.GalleryName;
               model.GalleryLocation = query.GalleryLocation;
               model.GalleryDescription = query.GalleryDescription;
               model.GalleryRemoteID = query.GalleryRemoteID;
               return View(model);
          }

          [HttpPost]
          public IActionResult Update(PersonalWebsiteMVC.Models.Gallery model)
          {
               if (ModelState.IsValid)
               {
                    var query = _db.Gallery.Where(u => u.GalleryRemoteID.Equals(model.GalleryRemoteID)).FirstOrDefault();
                    query.GalleryName = model.GalleryName;
                    query.GalleryLocation = model.GalleryLocation;
                    query.GalleryDescription = model.GalleryDescription;
                    _db.Gallery.Update(query);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Gallery");
               }
               return View();
          }

          public IActionResult Delete()
          {
               return View();
          }

          [HttpPost]
          [Route("Admin/Gallery/AddGalleryToDb()")]
          public IActionResult AddGalleryToDb(PersonalWebsiteMVC.Models.Gallery model)
          {
               var g = new PersonalWebsiteMVC.Models.Gallery();
               g.GalleryName = model.GalleryName;
               g.GalleryLocation = model.GalleryLocation;
               g.GalleryRemoteID = model.GalleryRemoteID;
               _db.Gallery.Add(g);

               var album = _Graph.GraphClient().Result.Drives["douglas@douglasmcgregor.co.uk"].Items[model.GalleryRemoteID].Children.Request().Expand("thumbnails").GetAsync().Result;
               for (int i = 0; i < album.Count(); i++)
               {
                    for (int p = 0; p < album[i].Thumbnails.Count(); p++)
                    {
                         var pic = new PersonalWebsiteMVC.Models.Photos();
                         pic.GalleryRemoteID = model.GalleryRemoteID;
                         pic.PhotoRemoteID = album[i].Id;
                         pic.PhotoName = album[i].Name;
                         pic.PhotoLocation = model.GalleryLocation;
                         _db.Photos.Add(pic);
                         _db.SaveChanges();
                    }
               }
               return RedirectToAction("Index", "Gallery");
          }
        
         
     }
}
