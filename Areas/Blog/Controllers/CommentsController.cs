using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
    public class CommentsController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }
        private IConfiguration _configuration { get; set; }
        private ILogger _Logger { get; set; }

        public CommentsController(ApplicationDbContext db, IHttpContextAccessor http, IConfiguration config, ILogger logger)
        {
            _db = db;
            _http = http;
            _configuration = config;
            _Logger = logger;
        }


        public IActionResult Index(int id)
        {
            var model = _db.Comments.Where(c => c.PostID == id).ToList();
            return View(model);
        }

        [Route("Comments/AddComment")]
        public IActionResult AddComment(BlogCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!ReCaptchaPassed(
                    Request.Form["g-recatcha-response"]!,
                    _configuration.GetSection("GoogleReCaptcha:secret").Value!,
                    _Logger
                  
                    ))
                {
                    Comments comment = new Comments();
                    comment.CommentAuthor = model.Comment.CommentAuthor;
                    comment.CommentAuthorEmail = model.Comment.CommentAuthorEmail;
                    comment.CommentAuthorUrl = model.Comment.CommentAuthorUrl;
                    comment.CommentContent = model.Comment.CommentContent;
                    comment.CommentDate = DateTime.Now;
                    comment.CommentAuthorIP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
                    comment.PostID = model.Comment.PostID;
                    comment.CommentType = "Post";
                    _db.Comments.Add(comment);
                    _db.SaveChanges();
                    return RedirectToAction("SinglePost", new { controller = "Home", area = "Blog", id = model.Comment!.PostID });
                }
                return View("~/Areas/Blog/Views/Shared/SinglePost", model);
            }
            return View("~/Areas/Blog/Views/Shared/SinglePost", model);
        }

        public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret, ILogger _Logger)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _Logger.LogError("Error while sending request to ReCaptcha");
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
    }
}
