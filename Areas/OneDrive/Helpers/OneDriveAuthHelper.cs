
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.OneDrive.Helpers
{
     public class OneDriveAuthHelper
     {
          private readonly IConfiguration _configuration;
          private readonly IHttpClientFactory _clientFactory;
          private readonly IHttpContextAccessor _http;

          public OneDriveAuthHelper(IConfiguration configuration, IHttpClientFactory clientFactory, IHttpContextAccessor http)
          {
               _configuration = configuration;
               _clientFactory = clientFactory;
               _http = http;
          }


          public async Task<string> GetAccessToken()
          {
               Random rnd = new Random();
               string clientId = _configuration["AzureAD:ClientId"]!;
               string clientSecret = _configuration["AzureAD:ClientSecret"]!;
               string tenantId = _configuration["AzureAD:TenantId"]!;

               HttpClient client = _clientFactory.CreateClient();
               HttpRequestMessage request = new(method: HttpMethod.Post, requestUri: $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token");

               request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
               {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "code_verifier", rnd.Next(1, 43).ToString() },
                    { "scope", "https://graph.microsoft.com/.default" },
                    { "code", _http.HttpContext!.Request.Query["code"]! },
                    { "redirect_uri", "http://localhost:5051/OneDrive/" },
                    { "grant_type", "client_credentials" }
               });


               HttpResponseMessage response = await client.SendAsync(request);
               //response.EnsureSuccessStatusCode();
               var body = await response.Content.ReadAsStringAsync();
               if (!response.IsSuccessStatusCode)
               {
                    throw new HttpRequestException(
                         $"Request failed ({(int)response.StatusCode}):{body}"
                    );
               }
               var json = JsonConvert.DeserializeObject<OneDriveAuth>(body);
               return json!.access_token;
          }

     }

}

