using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Views.Components
{
     public class Comments : ViewComponent
     {
          private ApplicationDbContext _db;
          public Comments(ApplicationDbContext db)
          {
               _db = db;
          }


		public IViewComponentResult Invoke(string commentType, string picId, string photoName, int postId)
		{
			if (commentType == "post")
			{
				var query = _db.Comments.Where(c => c.ParentID == postId).FirstOrDefault();;
				if (query == null)
				{
					return View("Default");
				}
			}
			return View();
		}
	
        
     }

}
