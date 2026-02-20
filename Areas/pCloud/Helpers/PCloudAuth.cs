using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Models;
using RestSharp;

namespace PersonalWebsiteMVC.Areas.pCloud.Helpers
{
     public interface IPCloudAuth
     {
          void Auth();
          string GetAccessToken();
     }

     public class PCloudAuth : IPCloudAuth
     {
          public IHttpContextAccessor _http { get; set; }
          public IWebHostEnvironment _env { get; set; }

          public PCloudAuth(IHttpContextAccessor http, IWebHostEnvironment env)
          {
               _http = http;
               _env = env;
          }

          public void Auth()
          {
               string clientId = "GJR8uDME26u";
               string url = string.Empty;
               if (_env.IsProduction())
               {
                    url = $"https://my.pcloud.com/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri=https://www.douglasmcgregor.co.uk/pCloud/";
               }
               else
               {
                    url = $"https://my.pcloud.com/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri=http://localhost:5051/pCloud/";
               }
               _http.HttpContext!.Response.Redirect(url);
          }

          public string GetAccessToken()
          {
               string clientId = "GJR8uDME26u";
               string clientSecret = "U83OQca6ABpaiDtaBsStUbgKRiAk";
               string url = "https://eapi.pcloud.com/";

               var client = new RestClient(url);
               var request = new RestRequest("oauth2_token");
               request.AddParameter("client_id", clientId);
               request.AddParameter("client_secret", clientSecret);
               request.AddParameter("code", _http.HttpContext.Request.Query["code"]);
               var response = client.Execute(request);
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
               Console.WriteLine(response.Content);
               var json = JsonConvert.DeserializeObject<pCloudToken>(response.Content!);
              
                    _http.HttpContext.Session.SetString("PCloudToken", json!.access_token!);

               return json!.access_token!;
          }
     }
}
