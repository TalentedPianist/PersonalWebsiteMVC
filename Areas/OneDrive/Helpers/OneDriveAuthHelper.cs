
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Microsoft.Graph;
using Azure.Identity;

namespace PersonalWebsiteMVC.Areas.OneDrive.Helpers
{
     public class OneDriveAuthHelper
     {
          // https://medium.com/@python-javascript-php-html-css/how-to-retrieve-and-use-graph-api-access-tokens-for-sending-emails-in-c-3604535d0b3e
     

          private static readonly HttpClient httpClient = new HttpClient();

          private string ClientID = "4cb4c3f8-b663-44bb-9f7c-ec3ee7e42edb";
          private string ClientSecret = "CNY8Q~ed7l0NnLEKyBYSPYimr3gd8dCqWmv0pbuG";
          private string TenantID = "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab";

          private readonly IPublicClientApplication _app;
          private readonly string[] _scopes = new[] { "User.Read" };

          public OneDriveAuthHelper()
          {
               _app = PublicClientApplicationBuilder.Create(ClientID)
                    .WithAuthority(AzureCloudInstance.AzurePublic, TenantID)
                    .WithRedirectUri("http://localhost:5051")
                    .Build();
          }


          // Fetch access token using Client Credentials flow
          public async Task<string> GetAccessToken()
          {
               
               var tokenEndpoint = $"https://login.microsoft.com/{TenantID}/oauth2/v2.0/token";
               // Prepare the request body
               var body = new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("client_id", ClientID),
                    new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                    new KeyValuePair<string, string>("client_secret", ClientSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
               });
               // Make the HTTP Post request
               HttpResponseMessage response = await httpClient.PostAsync(tokenEndpoint, body);
               response.EnsureSuccessStatusCode();
               // Read and parse the response
               string responseContent = await response.Content.ReadAsStringAsync();
               var tokenResult = JsonConvert.DeserializeObject<dynamic>(responseContent);
               return tokenResult!.access_token;

          }

          private static GraphServiceClient? _graphClient;

         
     }
}
