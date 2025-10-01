using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using Wangkanai.Extensions;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
    [Area("pCloud")]
    public class HomeController : Controller
    {
          private static readonly HttpClient client = new HttpClient();

        public async Task<IActionResult> Index([FromQuery(Name="code")]string code)
        {
               try
               {
                    if (code is not null)
                    {


                         HttpContext.Response.Cookies.Append("AccessToken", await GetAccessToken(code));
                    }
               }
               catch (ArgumentNullException ex)
               {
                    
               }
            return View();
        }

        public IActionResult Login()
        {
            string url = "https://my.pcloud.com/oauth2/authorize";
            string clientId = "GJR8uDME26u";
            string redirectUri = "http://localhost:5051/pCloud/";
            return Redirect($"{url}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&state=12345");
        }

          public async Task<string> GetAccessToken(string code)
          {
               string tokenUrl = "https://eapi.pcloud.com/oauth2_token";
               string client_id = "GJR8uDME26u";
               string client_secret = "U83OQca6ABpaiDtaBsStUbgKRiAk";
               string redirectUri = "http://localhost:5051/pCloud/";

               
               var values = new Dictionary<string, string>
               {
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", redirectUri },
                    { "client_id", client_id },
                    { "client_secret", client_secret },
                    { "code", code }
               };

               var content = new FormUrlEncodedContent(values);
               var response = await client.PostAsync(tokenUrl, content);
               var responseString = await response.Content.ReadAsStringAsync();

               var result = JsonConvert.DeserializeObject<TokenModel>(responseString);
               return result!.access_token!;
          }

       
    }
}
