using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;
using PersonalWebsiteMVC.Services;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class ContactController : ControllerBase
     {
          private EmailService _EmailService { get; set; }

          public ContactController(EmailService emailService)
          {
               _EmailService = emailService;
          }

          [HttpPost("/api/Contact")]
          public async Task<IActionResult> SendEmail(Contact model)
          {

               if (ModelState.IsValid)
               {
                    await _EmailService.SendEmailAsync("douglas@douglasmcgregor.co.uk", "Contact Form Enquiry", model.Message!);
               }
               return Ok(model);

          }
     }
}
