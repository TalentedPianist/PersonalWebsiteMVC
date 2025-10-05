using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;


namespace PersonalWebsiteMVC.Helpers
{
     public class OneDriveAuth 
     {

          public static string ClientID = "4cb4c3f8-b663-44bb-9f7c-ec3ee7e42edb";
          public static string ClientSecret = "VhF8Q~YaJrHk39hrm6Xpe03~D3zAiHwHTV1sBbBd";
          public static string TenantID = "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab"; 
          public static string? AccessToken { get; set; }


          public async Task<string> GetAccessToken()
          {


               var app = ConfidentialClientApplicationBuilder.Create(ClientID)
                    .WithClientSecret(ClientSecret)
                    .WithRedirectUri("http://localhost:5051/OneDrive/")
                    .Build();

                    string[] scopes = { "User.Read" };

               var result = app.AcquireTokenByAuthorizationCode(scopes, "");
                    
               
              
          }
          

          
     }
}
