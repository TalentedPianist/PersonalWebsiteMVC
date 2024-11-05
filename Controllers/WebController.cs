using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WebController(IConfiguration configuration, HttpClient httpClient, ApplicationDbContext db)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _db = db;
        }

        [HttpGet("Captcha")]
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

        public IActionResult Index()
        {
            return Ok(DateTime.Now);
        }

        [HttpGet("About")]
        public IActionResult About()
        {
            return Ok("Blah blah blah");
        }

        [HttpGet("Posts")]
        public IActionResult GetPosts()
        {
            List<Posts> posts = _db.Posts.ToList();
            return new JsonResult(posts);
        }

        [HttpGet("Comments")]
        public IActionResult GetComments()
        {
            List<Comments> comments = _db.Comments.ToList();
            return new JsonResult(comments);
        }

    }
}
