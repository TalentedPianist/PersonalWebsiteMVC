using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
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

      

        [HttpPost]
        [Route("/Desktop/Blog/AddComment")]
        public async Task<IActionResult> AddComment(string name, string email, string website, string message, string captchaResponse, int postID)
        {
            if (await VerifyCaptcha(captchaResponse))
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
            else
            {
                return Json(new { error = "Please do the captcha to prove that you are human." });
            }
        }

        private async Task<bool> VerifyCaptcha(string captchaResponse)
        {
            var secretKey = "6LeCBlUrAAAAACVipFQ2hXQkaRn1i_pFJEZIegge";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var captchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(jsonResponse);
                return captchaResult!.Success;
            }
            return false;
        }
    }
}
