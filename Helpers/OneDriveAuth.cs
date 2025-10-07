using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Helpers
{
     public class OneDriveAuth 
     {

          public static string ClientID = "4cb4c3f8-b663-44bb-9f7c-ec3ee7e42edb";
          public static string ClientSecret = "VhF8Q~YaJrHk39hrm6Xpe03~D3zAiHwHTV1sBbBd";
          public static string TenantID = "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab"; 
          public static string? AccessToken { get; set; }
          public string RedirectUri = "http://localhost:5051/oneDrive/";

          public IHttpClientFactory _httpClientFactory { get; set; }
          private IHttpContextAccessor _http { get; } 

          public OneDriveAuth(IHttpClientFactory httpClientFactory, IHttpContextAccessor http)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
          }


          public IActionResult GetCode()
          {
               string[] scope = { "User.Read" };
               string url = $"https://login.microsoftonline.com/{TenantID}/oauth2/v2.0/authorize?client_id={ClientID}&response_type=code&redirect_uri={RedirectUri}&response_mode=query&scope={scope}&state=12345";

               return new RedirectResult(url);
          }

          
     }
}
