using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using reCAPTCHA.AspNetCore.Attributes;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
     public class CommentsController : Controller
     {
          public ApplicationDbContext _db { get; set; }
          public IHttpContextAccessor _http { get; set; }
          private IConfiguration _configuration { get; set; }
          public ILogger<CommentsController> _Logger { get; set; }
          private readonly HttpClient _httpClient;

          public CommentsController(ApplicationDbContext db, IHttpContextAccessor http, IConfiguration config, ILogger<CommentsController> logger, HttpClient httpClient)
          {
               _db = db;
               _http = http;
               _configuration = config;
               _Logger = logger;
               _httpClient = httpClient;

          }



          [HttpPost]
          [Route("/Desktop/Blog/AddComment")]
          [ValidateRecaptcha(0.5)]
          public async Task<IActionResult> AddComment(string name, string email, string website, string message, string captchaResponse, int postID)
          {

               Comments comment = new Comments();
               comment.CommentAuthor = name;
               comment.CommentAuthorEmail = email;
               comment.CommentAuthorUrl = website;
               comment.CommentContent = message;
               comment.CommentAuthorIP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
               comment.CommentDate = DateTime.Now;
               comment.PostID = postID;
               _db.Comments.Add(comment);
               _db.SaveChanges();
               return Ok(comment);

          }

          
     }
}
