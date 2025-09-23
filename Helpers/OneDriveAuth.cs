using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace PersonalWebsiteMVC.Helpers
{
     public class OneDriveAuth 
     {


          public string GetAccessToken()
          {
               var scopes = new[] { "User.Read" };
               var tenantId = "6e07ac96-b10b-43e3-b9a4-37d7dfcddfab";
               var clientId = "4cb4c3f8-b663-44bb-9f7c-ec3ee7e42edb";
               var clientSecret = "VhF8Q~YaJrHk39hrm6Xpe03~D3zAiHwHTV1sBbBd";


               return DateTime.Now.ToString();
          }

          
     }
}
