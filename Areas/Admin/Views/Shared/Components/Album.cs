using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Helpers;

namespace PersonalWebsiteMVC.Areas.Admin.Views.Components
{
     public class Album : ViewComponent
     {
          public ApplicationDbContext _db;
          
          public Album(ApplicationDbContext db)
          {
               _db = db;
          }

          public IViewComponentResult Invoke(string picName, string albumId)
          {
               var query = _db.Photos.Where(p => p.PhotoName == picName && p.GalleryRemoteID == albumId).FirstOrDefault();
               if (query == null)
               {
                    var model = new PersonalWebsiteMVC.Models.Photos();
                    model.GalleryRemoteID = albumId;
                    model.PhotoName = picName;
                    return View("FormView1", model);
               }
               else
               {
                    var model = new PersonalWebsiteMVC.Models.Photos();
                    model.PhotoName = query.PhotoName;
                    model.PhotoLocation = query.PhotoLocation;
                    model.PhotoDescription = query.PhotoDescription;
                    return View("FormView2", model);
               }
          }
     }
}
