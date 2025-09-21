using Azure.Identity;
using Microsoft.Graph;

namespace PersonalWebsiteMVC.Helpers
{
     public class GraphHelper
     {
          private readonly IConfiguration _configuration;
          private readonly IHostEnvironment _environment;
          private GraphServiceClient? _graphServiceClient;

          public GraphHelper(IConfiguration configuration, IHostEnvironment environment)
          {
               _configuration = configuration;
               _environment = environment;
          }


          public async Task<string> AuthenticateMSGraph()
          {
               var client = new HttpClient();
               var parameters = new Dictionary<string, string>
               {
                    { "client_id", "4cb4c3f8-b663-44bb-9f7c-ec3ee7e42edb" },
                    { "scope", "https://graph.microsoft.com/.default" },
                    { "client_secret", "Z8X8Q~Ebc1AGtjKinmIOAmgaRrssvdJiPPVyKb50" },
                    { "grant_type", "client_credentials" },
                    { "tenant", "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab" }
               };

               var response = await client.PostAsync("https://login.microsoftonline.com/6e07ac96-b10b-43e3-b9a4-37d7dfcddfab/oauth2/v2.0/token", new FormUrlEncodedContent(parameters));

               var content = await response.Content.ReadAsStringAsync();
               return content;
          }
     }
}
