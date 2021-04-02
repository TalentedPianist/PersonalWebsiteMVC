using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
	public class PhotoController : Controller
	{
		private readonly IGraphSDKHelper _Graph;
		private readonly ApplicationDbContext _db;

		public PhotoController(IGraphSDKHelper graph, ApplicationDbContext db)
		{
			_Graph = graph;
			_db = db;
		}

		[HttpPost]
		[Route("/Admin/Gallery/RemovePicsFromDb")]
		public IActionResult RemovePicsFromDb(PersonalWebsiteMVC.Models.Photos model)
		{
			var query = from c in _db.Photos where c.GalleryRemoteID.Equals(HttpContext.Request.Query["q"]) select c;
			_db.RemoveRange(query);
			_db.SaveChanges();
			//return View("/Areas/Admin/Views/Gallery/Album.cshtml");
			return RedirectToAction("Album", "Gallery", new { q = HttpContext.Request.Query["q"], Area = "Admin" });
		}

		[HttpPost]
		[Route("/Admin/Photo/AddPicToDb()")]
		public IActionResult AddPicToDb(PersonalWebsiteMVC.Models.Photos model, IFormCollection form)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < form["PicName"].Count(); i++)
			{
				var p = new PersonalWebsiteMVC.Models.Photos();
				p.PhotoName = model.PhotoName;
				p.PhotoRemoteID = model.PhotoRemoteID;
				p.PhotoMediumUrl = model.PhotoMediumUrl;
				p.PhotoLargeUrl = model.PhotoLargeUrl;
				p.GalleryRemoteID = HttpContext.Request.Query["q"];
				_db.Photos.Add(p);
				_db.SaveChanges();
			}
			return RedirectToAction("Album", new { q = HttpContext.Request.Query["q"], Area = "Admin" });
		}

	}
}
