using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Helpers
{
     public interface IAuthHelper
     {
          Task<string> AccessToken();
     }

     public class AuthHelper : IAuthHelper
     {
          public string ClientID { get; set; } = "1d9ecccc-be65-4e04-bbf3-133d98688b84";
          public string ClientSecret { get; set; } = "F=_2l.RBH3eDVpYLA/TZFfadj6RToju6";
          public string TenantID { get; set; } = "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab";
          public string[] Scopes { get; set; } = new string[] { "https://graph.microsoft.com/.default" };


          public async Task<string> AccessToken()
          {

               string Authority = "https://login.microsoftonline.com/" + TenantID;
               IConfidentialClientApplication app;
               app = ConfidentialClientApplicationBuilder.Create(ClientID)
                    .WithAuthority(Authority)
                    .WithClientSecret(ClientSecret)
                    .WithRedirectUri("https://localhost:5000")
                    .Build();
              
               var result = await app.AcquireTokenForClient(Scopes).ExecuteAsync();
               return result.AccessToken;
          }
     }

   
}