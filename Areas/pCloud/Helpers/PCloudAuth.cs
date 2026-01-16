using Newtonsoft.Json;
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

          public PCloudAuth(IHttpContextAccessor http)
          {
               _http = http;
          }

          public void Auth()
          {
               string clientId = "GJR8uDME26u";
               string url = $"https://my.pcloud.com/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri=http://localhost:5051/pCloud/";
               _http.HttpContext!.Response.Redirect(url);
          }

          public string GetAccessToken()
          {
               string clientId = "GJR8uDME26u";
               string clientSecret = "U83OQca6ABpaiDtaBsStUbgKRiAk";
               string url = "https://eapi.pcloud.com/oauth2_token";

               var client = new RestClient(url);
               var request = new RestRequest();
               request.AddParameter("client_id", clientId);
               request.AddParameter("client_secret", clientSecret);
               request.AddParameter("code", _http.HttpContext.Request.Query["code"]);
               var response = client.Execute(request);
               var json = JsonConvert.DeserializeObject<pCloudToken>(response.Content!);
               Console.WriteLine(response.Content);
               return json!.access_token!;
          }
     }
}
