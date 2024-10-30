using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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


        public IActionResult Index(int id)
        {
            var model = _db.Comments.Where(c => c.PostID == id).ToList();
            return View(model);
        }

        [Route("Comments/AddComment")]
        [HttpPost]
        public IActionResult AddComment(BlogCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (ReCaptchaPassed(
                    Request.Form["g-recaptcha-response"]!,
                    _configuration.GetSection("reCaptcha:secret").Value!,
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
                    return RedirectToAction("SinglePost", new { controller = "Home", area = "Blog", id = model.Comment.PostID });
                }
                return View("~/Areas/Blog/Views/Shared/SinglePost.cshtml", model);
            }
            else
            {
                ModelState.AddModelError("", "Please check the recaptcha to prove that you're not a bot.");
                TempData["Message"] = Request.Form["g-recaptcha-response"];
            }
    
                return View("~/Areas/Blog/Views/Shared/SinglePost.cshtml");
           
        }


        public async Task<bool> GetreCaptchaResponse(string userResponse)
        {
            var reCaptchaSecretKey = _configuration["reCaptcha:SecretKey"];

            if (reCaptchaSecretKey != null && userResponse != null)
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "secret", reCaptchaSecretKey" },
                    { "response", userResponse }
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
