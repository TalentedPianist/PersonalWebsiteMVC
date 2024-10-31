using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
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

        [Route("Blog/Comments/AddComment/{id}")]
        public IActionResult Index(int id)
        {
            var model = new BlogCommentViewModel();
            model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            return View("~/Areas/Blog/Views/Shared/SinglePost.cshtml", model);
        }

        [Route("Blog/Comments/AddComment/{id}")]
        [HttpPost]
        public IActionResult AddComment(BlogCommentViewModel model)
        {
            if (ModelState.IsValid)
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
                    return RedirectToAction("SinglePost", new { controller = "Home", area = "Blog", id = model.Comment.PostID });
                
            }
            return View("~/Areas/Blog/Views/Shared/SinglePost.cshtml", model);
        }


        public async Task<bool> GetreCaptchaResponse(string userResponse)
        {
            var reCaptchaSecretKey = _configuration["reCaptcha:SecretKey"];
            if (reCaptchaSecretKey != null && userResponse != null)
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"secret", reCaptchaSecretKey },
                    {"response", userResponse }

                });
                var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<reCaptchaResponse>();
                    return result!.Success;
                }
            }
            return false;
        }
    }
}
