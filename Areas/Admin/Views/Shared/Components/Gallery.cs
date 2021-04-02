using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Areas.Admin.Views.Shared.Components
{
     public class Gallery : ViewComponent
     {
          public ApplicationDbContext _db { get; set; }
          public IHttpContextAccessor _http { get; set; }
          public IGraphSDKHelper _Graph { get; set; }

          public Gallery(ApplicationDbContext db, IHttpContextAccessor http, IGraphSDKHelper graph)
          {
               _db = db; 
               _http = http;
               _Graph = graph;
          }

          public IViewComponentResult Invoke(string galleryId, string galleryName)
          {
               var query = _db.Gallery.Where(g => g.GalleryRemoteID.Equals(galleryId) && g.GalleryName.Equals(galleryName)).FirstOrDefault();
               if (query == null)
               {
                    var model = new PersonalWebsiteMVC.Models.Gallery();
                    model.GalleryName = galleryName;
                    model.GalleryRemoteID = galleryId;
                    return View("FormView1", model);
               }
               else
               {    
                    var model = new PersonalWebsiteMVC.Models.Gallery();
                    model.GalleryName = query.GalleryName;
                    model.GalleryLocation = query.GalleryLocation;
                    model.GalleryDescription = query.GalleryDescription;
                    model.GalleryRemoteID = query.GalleryRemoteID;
                    return View("FormView2", model);
               }
          }
     }
}
