using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
     public class BlogController : Controller
     {
          private readonly ApplicationDbContext _db;
          private readonly IConfiguration _configuration;
          private static IHttpContextAccessor _http;

          public BlogController(ApplicationDbContext db, IConfiguration config, IHttpContextAccessor http)
          {
               _db = db; 
               _configuration = config;
               _http = http;
          }


          public IActionResult Index()
          {
               ViewBag.Current = "Blog";
               var model = new PersonalWebsiteMVC.Models.MixModel();
			if (HttpContext.Request.Query.ContainsKey("category"))
			{
				model.AllPosts = _db.Posts.Where(p => p.CategoryID == Convert.ToInt32(HttpContext.Request.Query["category"])).ToList();
				model.Posts = _db.Posts.Where(p => p.PostID == Convert.ToInt32(HttpContext.Request.Query["q"])).FirstOrDefault();
				model.AllComments = _db.Comments.Where(c => c.ParentID == Convert.ToInt32(HttpContext.Request.Query["q"])).ToList();
				return View(model);
			}
			else
			{
				
				model.AllPosts = _db.Posts.ToList();
				model.Posts = _db.Posts.Where(p => p.PostID == Convert.ToInt32(HttpContext.Request.Query["q"])).FirstOrDefault();
				model.AllComments = _db.Comments.Where(c => c.ParentID == Convert.ToInt32(HttpContext.Request.Query["q"])).ToList();
				return View(model);
			}
          }

          public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret)
          {
               HttpClient httpClient = new HttpClient();
               var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
               if (res.StatusCode != HttpStatusCode.OK)
               {
              
                    return false;
               }

               string JSONres = res.Content.ReadAsStringAsync().Result;
               dynamic JSONdata = JObject.Parse(JSONres);
               if (JSONdata.success != "true")
               {
                    return false;
               }

               return true;
          }

          [HttpPost]
          public IActionResult AddComment(MixModel model)
          {
               if (ModelState.IsValid)
               {
                    if (!ReCaptchaPassed(Request.Form["g-recaptcha-response"], _configuration.GetSection("GoogleReCaptcha:secret").Value))
                    {
                         ModelState.AddModelError(string.Empty, "You failed the CAPTCHA, stupid robot.  Go play some 1x1 on SFs instead.");
                       
                         return View("~/Views/Home/Blog.cshtml", model);
                    }

                    var comment = new Comments();
                    comment.ParentID = model.Comments.ParentID;
                    comment.CommentContent = model.Comments.CommentContent;
                    comment.CommentAuthor = model.Comments.CommentAuthor;
                    comment.CommentAuthorEmail = model.Comments.CommentAuthorEmail;
                    comment.CommentAuthorUrl = model.Comments.CommentAuthorUrl;
                    comment.CommentAuthorIP = HttpContext.Connection.RemoteIpAddress.ToString();
                    comment.CommentDate = DateTime.Now;
                    comment.CommentType = "Comment"; 
                    _db.Add(comment);
                    _db.SaveChanges();
                    if (model.Comments.CommentType == "post")
                    {
                         return RedirectToAction("Blog", new { q = model.Comments.ParentID });
                    }
                    else
                    {
                         return RedirectToAction("Gallery", new { q = model.Comments.PhotoID });
                    }
               }
               
               return View("~/Views/Home/Blog.cshtml?q=" + HttpContext.Request.Query["q"], model);
          }
     }
}
