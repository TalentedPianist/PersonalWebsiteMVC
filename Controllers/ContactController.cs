using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Models;
using PersonalWebsiteMVC.Services;

namespace PersonalWebsiteMVC.Controllers
{
     public class ContactController : Controller
     {
          private readonly IEmailSender _emailSender;
          private readonly IConfiguration _configuration;

          public ContactController(IEmailSender emailSender, IConfiguration config)
          {
               _emailSender = emailSender;
               _configuration = config;
          }

          public IActionResult Index()
          {
               ViewBag.Current = "Contact";
               return View();
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
          public async Task<IActionResult> Contact(ContactFormModel model)
          {
               if (ModelState.IsValid)
               {
                    if (!ReCaptchaPassed(Request.Form["g-recaptcha-response"], _configuration.GetSection("GoogleReCaptcha:secret").Value))
                    {
                         ModelState.AddModelError(string.Empty, "You failed the CAPTCHA, stupid robot.  Go play some 1x1 on SFs instead.");
                         return View("~/Views/Home/Contact.cshtml", model);
                    }
                    
                    var email = model.Email;
                    var subject = "Contact Form";
                    var message = model.Message;
                    var name = model.Name;
                    await _emailSender.SendEmailAsync(email, subject, message);
                    TempData["Message"] = "Thank you for your email.  A reply will be sent as soon as possible.";

                    return View();
               }
               
               return View("~/Views/Home/Contact.cshtml", model);
          }
     }
}
