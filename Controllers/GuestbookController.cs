using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Logging;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using Serilog;

namespace PersonalWebsiteMVC.Controllers
{
     public class GuestbookController : Controller
     {
          private readonly ApplicationDbContext _db;
          private readonly IConfiguration _configuration;

          public GuestbookController(ApplicationDbContext db, IConfiguration conf)
          {
               _db = db;
               _configuration = conf;
          }

          public IActionResult Index()
          {
               ViewBag.Current = "Guestbook";
               var model = new GuestbookMixModel();
               model.GuestbookAll = _db.Guestbook.Where(g => g.GuestbookApproved == "Yes").ToList();
               return View(model);
          }

          [HttpPost]
          public IActionResult AddEntry(GuestbookMixModel model)
          {
               if (ModelState.IsValid)
               {
                    if (ReCaptchaPassed(Request.Form["g-recaptcha-response"], _configuration.GetSection("GoogleReCaptcha:secret").Value))
                    {
                         var entry = new Guestbook();
                         entry.GuestbookComment = model.Guestbook.GuestbookComment;
                         entry.GuestbookUser = model.Guestbook.GuestbookUser;
                         entry.GuestbookUserEmail = model.Guestbook.GuestbookUserEmail;
                         entry.GuestbookUserWebsite = model.Guestbook.GuestbookUserWebsite;
                         entry.GuestbookDate = DateTime.Now;
                         entry.GuestbookIP = HttpContext.Connection.RemoteIpAddress.ToString();
                         entry.GuestbookApproved = "No";
                         _db.Guestbook.Add(entry);
                         _db.SaveChanges();
                         return RedirectToAction("Index");
                    }
                    else
                    {
                         ModelState.AddModelError(string.Empty, "Sorry, you failed the ReCaptcha check.");
                         return View(model);
                    }

               }
               return View("Index");
          }

          public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret)
          {
               var log = new LoggerConfiguration()
                    .WriteTo.File("C:\\log.txt")
                    .CreateLogger();

               HttpClient httpClient = new HttpClient();
               try
               {
                    var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
                    if (res.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                         log.Information(res.Content.ReadAsStringAsync().Result);
                         return false;
                    }

                    string JSONres = res.Content.ReadAsStringAsync().Result;
                    dynamic JSONdata = JObject.Parse(JSONres);
                    if (JSONdata.success != "true")
                    {

                         return false;
                    }
               }
               catch (HttpRequestException e)
               {
                    log.Information(e.Message);
               }

               return true;
          }
     }
}
