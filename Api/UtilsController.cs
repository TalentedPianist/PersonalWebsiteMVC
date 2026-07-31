using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class UtilsController : ControllerBase
     {
          [HttpGet("/api/RecaptchaVerify")]
          public async Task<IActionResult> CheckRecaptcha(string captchaToken, string secretKey)
          {
               var client = new RestClient("https://www.google.com/recaptcha/api");
               var request = new RestRequest("/siteverify");
               request.AddParameter("secret", secretKey);
               request.AddParameter("response", captchaToken);
               var response = await client.ExecuteAsync(request);
               return Ok(response.Content);
          }
     }
}
