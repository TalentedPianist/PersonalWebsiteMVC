
using BlazorPagination;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using X.PagedList.Extensions;

public class GuestbookController : Controller
{
    private ApplicationDbContext _db { get; set; }
    private IHttpContextAccessor _http { get; set; }

    public GuestbookController(ApplicationDbContext db, IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    [Route("/Home/Guestbook")]
    public IActionResult Guestbook([FromQuery(Name="pageNumber")]int? page)
    {
          var pageNumber = page ?? 1;
          var model = _db.Guestbook.ToPagedList(pageNumber, 3);
        return View("~/Views/Home/Guestbook.cshtml", model);
    }

    [HttpPost]
    [Route("/Guestbook/AddComment")]
    public async Task<IActionResult> AddComment(string name, string email, string website, string message, string captchaResponse)
    {
        if (await VerifyCaptcha(captchaResponse))
        {
            Guestbook guestbook = new Guestbook();
            guestbook.GuestbookAuthor = name;
            guestbook.GuestbookAuthorEmail = email;
            guestbook.GuestbookAuthorUrl = website;
            guestbook.GuestbookContent = message;
            guestbook.GuestbookAuthorIP = _http.HttpContext!.Connection.RemoteIpAddress!.ToString();
            guestbook.GuestbookDate = DateTime.Now;
            _db.Guestbook.Add(guestbook);
            _db.SaveChanges();
            return Ok(guestbook);
        }
        else
        {
            return Json(new { error = "You must do the captcha to prove that you are human." });
        }
    }

    public async Task<bool> VerifyCaptcha(string captchaResponse)
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